<template>
  <v-container class="white pa-1" style="width:100%;height:100%;user-select:none;">
    <v-row no-gutters style="height:40%" align="end">
      <v-col class="title font-weight-bold text-center">
          {{takeOutName}}
      </v-col>
    </v-row>
    <v-row no-gutters style="height:60%" align="center">
      <v-col class="text-center">
          <div class="display-1 font-weight-bold">
            {{takeOutCount}}
          </div>
          <div class="caption grey--text">
            件
          </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: 'TakeAway',
  props: {
    takeOutName: String,
    takeOutCount: Number
  },

  data: () => ({

  })
}
</script>
