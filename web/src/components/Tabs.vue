<template>
    <div class="tabs-component">
        <div class="tabs-header">
            <div
                v-for="tab in tabs"
                :key="tab.key"
                :class="['tab-btn', { active: tab.key === currTab }]"
                @click="setActive(tab.key)"
            >
                {{ tab.label }}
            </div>
        </div>
        <div class="tab-content">
            <slot :name="currTab"></slot>
        </div>
    </div>
</template>

<script>
export default {
    name: 'Tabs',
    emits: ['activeTab'],
    props: {
        tabs: {
            type: Array,
            required: true,
        },
        initialTab: {
            type: String,
            default: null,
        },
    },
    data() {
        return {
            currTab: null,
        };
    },
    mounted() {
        if(!this.currTab){
            this.currTab = this.initialTab || this.tabs[0].key;
            this.$emit('activeTab', this.currTab);
        }
    },
    methods: {
        setActive(tabKey) {
            this.currTab = tabKey;
            this.$emit('activeTab', tabKey);
        },
    },
};
</script>

<style>
.tabs-header {
    display: flex;
    border-bottom: 1px solid #ccc;
}
.tab-btn {
    padding: 10px 20px;
    cursor: pointer;
}

.tab-btn.active {
    border-bottom: 2px solid #0385db;
    font-weight: bold;
}

.tab-content {
    padding: 20px;
}

</style>