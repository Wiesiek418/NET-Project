<template>
  <div class="container">
    <header>
      <nav>
        <router-link to="/">Home</router-link> | 
        <router-link to="/about">About</router-link>
      </nav>
    </header>
    <main>
      <div>
        <h1>WELCOME ON BOARD</h1>
        <h2>HELLO WORLD</h2>
        <slot name="main_content"></slot>
      </div>
    </main>
  </div>
</template>

<script>
export default {
  name: 'TheContainer'
}
</script>