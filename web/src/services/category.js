import ApiService from './api.js';

class CategoryService {
    async getCategory(category, filter, sort) {
        let url = `/${category}?`;
        if (filter) {
            url += `filter=${convertFilters(filter)}&`;
        }
        if (sort) {
            url += `sort=${convertSort(sort)}&`;
        }
        const response =  await ApiService.get(url);
        return response;
    }
}

export default new CategoryService()