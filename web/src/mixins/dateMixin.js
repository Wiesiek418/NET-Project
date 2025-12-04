export default{
    methods:{
        parseDateTime(dateTime){
            const cleaned = dateTime.replace(/\.(\d{3})\d+/, '.$1'); 
            const date = new Date(cleaned);
            return date.toLocaleString();
        }
    }
}