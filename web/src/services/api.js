const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

class ApiService {
  constructor() {
    this.baseURL = API_BASE_URL
  }

  async request(endpoint, options = {}) {
    const {
      method = 'GET',
      body = null,
      headers = {},
      ...fetchOptions
    } = options

    const url = `${this.baseURL}${endpoint}`
    
    const config = {
      method,
      headers: {
        'Content-Type': 'application/json',
        ...headers
      },
      mode: 'cors',
      credentials: 'omit',
      ...fetchOptions
    }

    if (body && typeof body === 'object') {
      config.body = JSON.stringify(body)
    }

    try {
      const response = await fetch(url, config)
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      
      return await response.json()
    } catch (error) {
      throw error
    }
  }

  async get(endpoint, options = {}) {
    console.log('GET request to:', endpoint)
    return this.request(endpoint, { ...options, method: 'GET' })
  }

  async download(endpoint, options = {}, filename, format='json'){
    if(!filename){
      filename = 'download_' + Date.now();
    }
    const url = `${this.baseURL}${endpoint}`;
    const headers = {
      Accept: format === 'json' ? 'application/json' : 'text/csv',
      ...(options.headers || {})
    }

    try {
      const response = await fetch(url, {
        method: 'GET',
        mode: 'cors',
        credentials: 'omit',
        ...options,
        headers
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      let blob;
      if(format === 'json'){
        const data = await response.json();
        blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
      } else {
        const text = await response.text();
        blob = new Blob([text], { type: 'text/csv' });
      }

      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = filename + (format === 'json' ? '.json' : '.csv');
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

    }
    catch(error){
      throw error;
    }
  }
}

export default new ApiService()