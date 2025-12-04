<template>
    <div class="filter-sort-panel">
        <div class="filter-select-panel">
            <div
                v-if="!disabledFilters"
            >
                <div
                    v-for="(text, key) in headers"
                    :key="value"
                    v-if="!excludedFilters.includes(key)"
                    class="filter-container"
                >
                    <label class="label" :for="key">{{ text }}:</label>
                    <input
                        :id="key"
                        type="text"
                        :disabled="disabled"
                        v-model="filterValues[key]"
                        class="filter-select"
                    />
                </div>
            </div>
            <div
                v-if="!disabledSorts"
                class="filter-container"
            >
                <label class="label">Sort:</label>
                <select
                    :disabled="disabled"
                    v-model="selectedSort"
                    class="filter-select"
                >
                    <option 
                        v-for="option in sortOptions"
                        :value="option"
                        :key="option.direction + option.field"
                    >
                        {{ option.text }}
                    </option>
                </select>
            </div>
        </div>
        <div>
            <button 
                :disabled="disabled"
                @click="applyFiltersAndSorts()"
                class="filter-apply-btn"
            >
                Apply
            </button>
        </div>
    </div>
</template>

<script>
export default {
    name: 'FilterSortPanel',
    emits: ['apply'],
    props: {
        headers: {
            type: Object,
            required: true,
        }, 
        excludedFilters: {
            type: Array,
            default: () => [],
        },
        excludedSorts: {
            type: Array,
            default: () => [],
        },
        disabled: {
            type: Boolean,
            default: false,
        },
        disabledFilters:{
            type: Boolean,
            default: false,
        },
        disabledSorts:{
            type: Boolean,
            default: false,
        },
    },
    data() {
        return {
            sortOptions: [],
            selectedSort: null,
            filterValues: {},
        };
    },
    mounted() {
        this.setSortOptions();
    },
    methods: {
        setSortOptions() {
            this.sortOptions = Object.entries(this.headers)
                .filter(([key, text]) => !this.excludedSorts.includes(key))
                .flatMap(([key, text]) => ([
                    {
                        text: `${text} (Asc)`,
                        field: key,
                        direction: `asc`,
                    },
                    {
                        text: `${text} (Desc)`,
                        field: key,
                        direction: `desc`,
                    },
                ]));
        },
        applyFiltersAndSorts() {
            this.$emit('apply', {
                filters: this.filterValues,
                sort: this.selectedSort,
            });
        },
    }
};
</script>

<style>
.label{
    margin-right: 1.5rem;
    font-weight: bold;
    width: 150px;
}
.filter-container{
    display: flex;
    padding: 3px;
}
.filter-select{
    width: 150px;
    padding: 2px;
}
.filter-select-panel{
    margin: 10px 0;
}
.filter-apply-btn{
    background-color: var(--theme-color-thirdly);
    font-weight: bold;
}
</style>