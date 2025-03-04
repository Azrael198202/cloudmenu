<template>
  <v-app style="user-select:none">
    <router-view/>
  </v-app>
</template>

<script>

export default {
  name: 'App',

  data: () => ({
    //
  })
}
</script>
<style scoped>
  @import './assets/styles/developHelper.css';
  @import './assets/styles/lwStyle.css';
</style>
