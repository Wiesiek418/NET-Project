import * as signalR from '@microsoft/signalr';

class WebSocket{
    constructor(){
        this.connection = null;
        this.listeners = new Map();
    }

    async connect(url){
        if(this.connection){
            console.warn('WebSocket is already connected');
            return;
        }

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url) // backend hub endpoint
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on('NewReading', (message) => {
            this.listeners.forEach((callback) => callback(message));
        });

        try {
            await this.connection.start();
            console.log('SignalR connected.');
        } catch (err) {
            console.error('SignalR connection failed: ', err);
        }
    }

    subscribe(channel){
        const id = crypto.randomUUID();
        this.listeners.set(id, (data) => {
            if(data.channel === channel){
                console.log(`Message for channel ${channel}:`, data);
            }
        });
        return () => {
            this.listeners.delete(id);
        }
    }
}

export default WebSocketService = new WebSocket();