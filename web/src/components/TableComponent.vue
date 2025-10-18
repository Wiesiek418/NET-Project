<template>
  <div class="table-container">
    <table class="table">
      <thead>
        <tr>
          <th v-for="(header, index) in headerList" :key="index">
            {{ header.label }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(row, rowIndex) in data" :key="rowIndex">
          <td v-for="(header, colIndex) in headerList" :key="colIndex">
            <slot
              :name="`cell-${header.key}`"
              :value="row[header.key]"
              :row="row"
            >
              {{ row[header.key] }}
            </slot>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>

export default {
    name: 'TableComponent',
    props: {
        headers: {
            type: [Array, Object],
            required: true
        },
        data: {
            type: Array,
            required: true
        }
    },
    computed: {
        headerList() {
        if (Array.isArray(this.headers)) {
            return this.headers.map(h => ({
            key: h,
            label: h
            }));
        }

        if (typeof this.headers === "object") {
            return Object.entries(this.headers).map(([key, label]) => ({
            key,
            label
            }));
        }

        return [];
        }
    }
}
</script>

<style scoped>
.table-container {
  overflow-x: auto;
  padding: 1rem;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

th {
  background-color: #f5f5f5;
  font-weight: bold;
}
</style>