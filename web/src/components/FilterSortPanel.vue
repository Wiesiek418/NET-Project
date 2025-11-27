<template>
    <div class="filter-sort-panel">
        <div
            v-if="!disabledFilters"
        >
            <div
                v-for="header in headers"
                :key="header.value"
                v-if="!excludedFilters.includes(header.value)"
            >
                <!-- Filter UI elements go here -->
                <label :for="header.value">{{ header.text }}</label>
                <input
                    :id="header.value"
                    type="text"
                    :disabled="disabled"
                    v-model="filterValues[header.value]"
                />
            </div>
        </div>
        <div
            v-if="!disabledSorts"
        >
            <!-- Sort UI elements go here -->
            <label>Sort</label>
            <select
                :disabled="disabled"
                v-model="selectedSort"
            >
                <option 
                    v-for="option in sortOptions"
                    :value="option.value"
                    :key="option.value"
                >
                    {{ option.text }}
                </option>
            </select>
        </div>
        <div>
            <button 
                :disabled="disabled"
                @click="applyFiltersAndSorts()"
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
            type: Array,
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
            type: boolean,
            default: false,
        },
        disabledSorts:{
            type: boolean,
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
    method: {
        setSortOptions() {
            this.sortOptions = this.headers
                .filter(header => !this.excludedSorts.includes(header.value))
                .flatMap(header => ([
                    {
                        text: `${header.text} (Asc)`,
                        field: header.value,
                        direction: `asc`,
                    },
                    {
                        text: `${header.text} (Desc)`,
                        field: header.value,
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

</style>