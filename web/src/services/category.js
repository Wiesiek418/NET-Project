import ApiService from './api.js';
import { convertFilters, convertSort } from '../script/filterSortConverter.js';


class CategoryService {
    async getCategory(category, filter, sort) {
        let url = `/sensors/${category}?`;
        if (filter) {
            url += `filter=${convertFilters(filter)}&`;
        }
        if (sort) {
            url += `sort=${convertSort(sort)}&`;
        }
        const response =  await ApiService.get(url);
        return response;
    }

    async getSensors(filter, sort) {
        let url = `/sensors?`;
        if (filter) {
            url += `filter=${convertFilters(filter)}&`;
        }
        if (sort) {
            url += `sort=${convertSort(sort)}&`;
        }
        const response =  await ApiService.get(url);
        return response;
    }

    async getSensorById(category, sensorId) {
        let url = `/sensors/${category}?`;
        const filter = { Id: `=${sensorId}` };
        url += `filter=${convertFilters(filter)}`;

        const response =  await ApiService.get(url);
        return response;
    }

    async downloadCategory(category, filter, sort, filename, format='json') {
        let url = `/sensors/${category}?`;
        if (filter) {
            url += `filter=${convertFilters(filter)}&`;
        }
        if (sort) {
            url += `sort=${convertSort(sort)}&`;
        }
        await ApiService.download(url, {}, filename, format);
    }

    async downloadSensors(filter, sort, filename, format='json') {
        let url = `/sensors?`;
        if (filter) {
            url += `filter=${convertFilters(filter)}&`;
        }
        if (sort) {
            url += `sort=${convertSort(sort)}&`;
        }
        await ApiService.download(url, {}, filename, format);
    }

}

export default new CategoryService()