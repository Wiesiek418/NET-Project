import ApiService from './api.js';
import { convertFilters, convertSort } from '../script/filterSortConverter.js';


class WalletService {
    async getBalances() {
        let url = `/wallet/balances`;
        const response =  await ApiService.get(url);
        return response;
    }
    async getWallets() {
        let url = `/wallet`;
        const response =  await ApiService.get(url);
        return response;
    }

}

export default new WalletService()