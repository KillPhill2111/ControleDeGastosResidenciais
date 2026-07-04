import axios from 'axios';

const api=axios.create({
    baseURL:'http://localhost:5041/api',
});

export default api;