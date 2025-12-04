import * as signalR from '@microsoft/signalr';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

class WebSocket{
    constructor(){
        this.connection = null;
        this.listeners = new Map();
        this.baseURL = API_BASE_URL
    }

    async connect(endpoint){
        if(this.connection){
            console.warn('WebSocket is already connected');
            return;
        }
        const url = `${this.baseURL}${endpoint}`;
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url) // backend hub endpoint
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on('NewReading', (message) => {
            // console.log('SignalR msg:', message);
            this.listeners.forEach((callback) => callback(message));
        });

        try {
            await this.connection.start();
            console.log('SignalR connected.');
        } catch (err) {
            console.error('SignalR connection failed: ', err);
        }
    }

    subscribe(callback){
        const id = crypto.randomUUID();
        this.listeners.set(id,(data) => callback(data));
        return () => {
            this.listeners.delete(id);
        }
    }
}

export const webSocket = new WebSocket();
export default webSocket;