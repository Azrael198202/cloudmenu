<template>
  <div class="dashboard-container">
    <el-button @click="gotoDemo">测试</el-button>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import adminDashboard from "./admin";

export default {
  name: "Dashboard",
  components: { adminDashboard },
  data() {
    return {
      currentRole: "adminDashboard"
    };
  },
  computed: {
    ...mapGetters(["roles"])
  },
  created() {
    if (!this.roles.includes("admin")) {
      this.currentRole = "editorDashboard";
    }
  },
  methods: {
    gotoDemo() {
      this.$router.push("/demoPage");
    }
  }
};
</script>
