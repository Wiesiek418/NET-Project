<template>
    <footer class="footer-container">
        <p>© 2025 WiesiekCorp. All rights reserved.</p>
    </footer>
</template> 

<script>
export default {
    name: 'FooterComponent'
}
</script>

<style scoped>
.footer-container {
    height: 40px;
    font-weight: var(--font-weight-bold);
    background-color: var(--theme-color-main);
    padding: 10px;
    text-align: center;
    color: var(--theme-color-font-primary);
    display: flex;
    align-items: center;
    justify-content: center;
}
</style>