<template>
    <header class="header-container">
        <h1>My Application Header</h1>
    </header>
</template>

<script>
export default {
    name: 'HeaderComponent'
}
</script>

<style scoped>
.header-container {
    background-color: #dcf54f;
    padding: 20px;
    color: #00b600;
    display: flex;
    align-items: start;
    justify-self: center;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    font-weight: bold;
    font-size: 1.2rem;
}
</style>