import ApiService from './api.js';
import { convertFilters, convertSort } from '../script/filterSortConverter.js';


class WalletService {
    async getWallets() {
        let url = `/wallet/balances`;
        const response =  await ApiService.get(url);
        return response;
    }

}

export default new WalletService()