import ApiService from './api.js';
import { convertFilters, convertSort } from '../script/filterSortConverter.js';


class CategoryService {
    async getCategory(category, filter, sort) {
        let url = `/${category}?`;
        if (filter) {
            url += `filter=${convertFilters(filter)}&`;
        }
        if (sort) {
            url += `sort=${convertSort(sort)}&`;
        }
        console.log(url);
        const response =  await ApiService.get(url);
        return response;
    }
}

export default new CategoryService()