<template>
  <div id="app">
    <commonHeader></commonHeader>
    <!-- 导航结束 -->
    <div>
      <router-view :key="key"/>
    </div>
    <commonFooter></commonFooter>
  </div>
</template>

<script>
import commonHeader from "@/layout/commonHeader.vue";
import commonFooter from "@/layout/commonFooter.vue";
export default {
  name: "Layout",
  components: {
    commonHeader,
    commonFooter
  },
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
