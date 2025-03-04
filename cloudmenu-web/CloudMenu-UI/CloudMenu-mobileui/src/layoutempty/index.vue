<template>
  <div id="app">
    <router-view :key="key"/>
  </div>
</template>

<script>
export default {
  name: "LayoutEmpty",
  components: {},
  computed: {
    key() {
      return this.$route.name !== undefined
        ? this.$route.name + new Date()
        : this.$route + new Date();
    }
  },
  methods: {}
};
</script>

<style lang="scss" scoped>
</style>
