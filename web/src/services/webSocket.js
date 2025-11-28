class WebSocket{
    constructor(){
        this.socket = null;
        this.listeners = new Map();
    }

    connect(url){
        if(this.socket){
            console.warn('WebSocket is already connected');
            return;
        }

        this.socket = new WebSocket(url);

        this.socket.onopen = () => {
            console.log('WebSocket connection established');
        };

        this.socket.onmessage = (event) => {
            let data;
            try{
                data = JSON.parse(event.data);
            }
            catch{
                data = event.data;
            }

            this.listeners.forEach((callback) => callback(data));
            console.log('Message received:', event.data);
        };

        this.socket.onclose = () => {
            console.log('WebSocket connection closed');
        };

        this.socket.onerror = (error) => {
            console.error('WebSocket error:', error);
        };
    }

    sendMessage(message){
        if(this.socket && this.socket.readyState === WebSocket.OPEN){
            this.socket.send(JSON.stringify(message));
        } else {
            console.error('WebSocket is not open. Unable to send message.');
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