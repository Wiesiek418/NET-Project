<template>
  <TheContainer>
    <template #main_content>
      <div class="wallet-container">
          <h1>Blockchain wallets</h1>
          <button @click="fetchData()">Refresh</button>
          <tableComponent
              v-if="data"
              :headers="headers"
              :data="data"
              :key="`wallet_table_${refreshTable}`"
          >
          </tableComponent>
      </div>
    </template>
  </TheContainer>
</template>

<script>
import TheContainer from "../containers/TheContainer.vue";
import TableComponent from "../../components/TableComponent.vue";
import WalletService from '@/services/wallet.js';

export default {
  name: 'Blockchain',
  components: {
    TheContainer,
    TableComponent,
  },
  data(){
    return {
        data: null,
        headers: {
            SensorId: "Sensor ID",
            SensorType: "Category",
            Balance: "Balance (ETH)",
        },
        refreshTable: false,
    };
  },
  mounted(){
    this.fetchData();
  },
  methods:{
    async fetchData(){
        this.data = await WalletService.getBalances();
    },
    handleButton(){
        this.fetchData();
        this.refreshTable != this.refreshTable;
    }
  }
};
</script>