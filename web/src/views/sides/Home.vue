<template>
    <TheContainer>
        <template v-slot:main_content>
            <Tabs
                :tabs="tabs"
                @activeTab="activeTab"
            >
                <template #all>
                    <div class="home-sensors-container">
                        <div class="download-btn-container">
                            <button
                                @click="downloadData('json')"
                                class="download-btn"
                            >
                                Download Data JSON
                            </button>
                            <button
                                @click="downloadData('csv')"
                                class="download-btn"
                            >
                                Download Data CSV
                            </button>
                        </div>

                        <FilterSortPanel
                            :headers="headers"
                            @apply="fetchData"
                        />
                    
                        <tableComponent
                            v-if="sensorsListData"
                            :headers="headers"
                            :data="sensorsListData"
                        >
                        </tableComponent>
                    </div>
                </template>
                <template
                    v-for="category in categories"
                    #[category]
                >   
                    <SensorsView
                        :category="category"
                    />
                </template>
            </Tabs> 
        </template>
    </TheContainer>
</template>

<script>
import TableComponent from "../../components/TableComponent.vue";
import TheContainer from "../containers/TheContainer.vue";
import ChartView from "../../components/ChartView.vue";
import Tabs from "../../components/Tabs.vue";
import SensorsView from "../SensorsView.vue";
import CategoryService from '@/services/category';
import FilterSortPanel from "@/components/FilterSortPanel.vue";
import dateMixin from "@/mixins/dateMixin";

export default {
    name: 'Home',
    mixins: [dateMixin],
    components: {
        TheContainer,
        TableComponent,
        ChartView,
        Tabs,
        SensorsView,
        FilterSortPanel,
    },
    data() {
        return {
            sensorsListData: [],
            chartData: {
                labels: [],
                datasets: []
            },
            headers: {
                sensorId: "Sensor ID",
                sensorType: "Category",
                createdAt: "Created at",
            },
            tabs:[
                { key: 'all', label: 'All' },
                { key: 'baking', label: 'Baking' },
                { key: 'conveyor', label: 'Conveyor' },
                { key: 'dough', label: 'Dough' },
                { key: 'packing', label: 'Packing' },
            ],
            categories:[
                'baking',
                'conveyor',
                'dough' ,
                'packing',
            ],
            activeTab: 'all',
            filters: {},
        };
    },
    mounted(){
        this.fetchData();
    },
    methods:{
        async fetchData(filters={}) {
            this.filters = filters;
            this.sensorsListData = await CategoryService.getSensors(this.filters.filters, this.filters.sort);
        },
        async downloadData(format) {
            await CategoryService.downloadSensors(this.filters.filters, this.filters.sort, `sensors_list_data`,format);
        }
    }
};
</script>
<style>
.download-btn-container{
    margin: 5px 0;
}
.download-btn{
    margin-right: 4px;
    background-color: var(--theme-color-thirdly);
    font-weight: bold;
}
</style>