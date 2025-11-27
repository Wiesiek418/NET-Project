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
}

export default new ApiService()