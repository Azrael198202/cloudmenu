<template>
  <v-container class="white pa-1" style="width:100%;height:100%;user-select:none;">
    <v-row no-gutters style="height:40%" align="end">
      <v-col class="title font-weight-bold text-center">
          {{sumName}}
      </v-col>
      <v-col class="subtitle-2 grey--text">
          {{sumCount}}室
      </v-col>
    </v-row>
    <v-row no-gutters style="height:60%" align="center">
      <v-col class="text-center">
          <div class="display-1 font-weight-bold">
            {{sumEmptyCount}}
          </div>
          <div class="caption grey--text">
            空
          </div>
      </v-col>
      <v-col class="text-center">
          <div class="display-1 font-weight-bold green--text">
            {{sumUsingCount}}
          </div>
          <div class="caption grey--text">
            使用中
          </div>
      </v-col>
      <v-col class="text-center">
          <div class="display-1 font-weight-bold red--text">
            {{sumNotifyCount}}
          </div>
          <div class="caption grey--text">
            警告
          </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: 'RooSum',
  props: {
    sumName: String,
    sumCount: Number,
    sumEmptyCount: Number,
    sumUsingCount: Number,
    sumNotifyCount: Number
  },
  data: () => ({

  })
}
</script>
