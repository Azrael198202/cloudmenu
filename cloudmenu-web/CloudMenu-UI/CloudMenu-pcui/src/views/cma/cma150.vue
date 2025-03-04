<template>
  <div class="container">
    <div class="main">
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>座席番号</span></el-col>
        <el-col :span="4">
          <el-select v-model="form.tableInfo">
            <el-option
              v-for="item in form.tableList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
        <el-col :span="3" :offset="2"><el-button type="danger" icon="el-icon-search">搜索</el-button></el-col>
      </el-row>
      <el-row style="margin-top:20px">
        <el-col :span="24">
          <el-card>
            <el-table :data="form.tableDataAll" height="300">
              <el-table-column label="商品名" width="250">
                <template slot-scope="scope">
                  <span>{{ scope.row.foodName }}</span>
                </template>
              </el-table-column>
              <el-table-column label="個数" width="120" align="center">
                <template slot-scope="scope">
                  <p>{{ scope.row.num }}</p>
                </template>
              </el-table-column>
              <el-table-column label="金額(税込)" width="180" align="center">
                <template slot-scope="scope">
                  <p>￥ {{ scope.row.moneyZeikomi }}</p>
                </template>
              </el-table-column>
              <el-table-column label="適用利率" width="120" align="center">
                <template slot-scope="scope">
                  <p>{{ scope.row.rate }} %</p>
                </template>
              </el-table-column>
              <el-table-column label="取消" width="120" align="center">
                <el-button size="mini">
                  <i class="fa fa-times" aria-hidden="true"></i>
                </el-button>
              </el-table-column>
              <el-table-column label="会計" width="120" align="center">
                <template slot-scope="scope">
                  <el-checkbox v-model="scope.row.checked"></el-checkbox>
                </template>
              </el-table-column>
            </el-table>
          </el-card>
        </el-col>
      </el-row>
      <el-row style="margin-top:20px" align="right">
        <el-col :span="2" :offset="22"><el-button type="danger">計算</el-button></el-col>
      </el-row>
      <el-row>
        <el-col :span="13">
          <el-card>
            <el-table :data="form.tableDataChecked" style="width: 100%">
              <el-table-column prop="foodNameChecked" label="商品名" width="180"></el-table-column>
              <el-table-column prop="numChecked" label="個数"></el-table-column>
              <el-table-column prop="moneyZeikomiChecked" label="金額(税込)"></el-table-column>
              <el-table-column prop="rateChecked" label="適用利率"></el-table-column>
            </el-table>
            <el-row class="sumRow">
              <el-col :span="8" style="text-align: right">合計</el-col>
              <el-col :span="3" :offset="1">6</el-col>
              <el-col :span="3" :offset="3">6840</el-col>
            </el-row>
            <el-row class="sumRow">
              <el-col :span="8" style="text-align: right">(8%対象</el-col>
              <el-col :span="3" :offset="1">￥699999</el-col>
              <el-col :span="1" :offset="3">税</el-col>
              <el-col :span="3">6840</el-col>
            </el-row>
            <el-row class="sumRow">
              <el-col :span="8" style="text-align: right">(10%対象</el-col>
              <el-col :span="3" :offset="1">￥699999</el-col>
              <el-col :span="1" :offset="3">税</el-col>
              <el-col :span="3">6840</el-col>
            </el-row>
          </el-card>
        </el-col>
      </el-row>
      <el-row style="margin-top:20px">
        <el-col :span="2"><el-button type="primary">支払方法</el-button></el-col>
      </el-row>
      <el-row style="margin-top:20px">
        <el-col :span="2">
          <span>現金</span>
          <i class="fa fa-money fa-5x" aria-hidden="true"></i>
        </el-col>
        <el-col :span="20" :offset="2">
          <el-image style="width: 100%; height: 150px" :src="form.url">
            <div slot="error" class="el-image__error"></div>
          </el-image>
        </el-col>
      </el-row>
      <el-row style="margin-top:20px">
        <el-col :span="2" :offset="18"><el-button type="primary">レシート</el-button></el-col>
        <el-col :span="2" :offset="2"><el-button type="danger">会計</el-button></el-col>
      </el-row>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      form: {
        tableInfo: ' ',
        tableList: [
          {
            value: '0',
            label: '個室一'
          },
          {
            value: '1',
            label: '個室二'
          },
          {
            value: '1',
            label: '個室三'
          }
        ],
        tableDataAll: [
          {
            foodName: '06 水煮牛肉',
            num: '1',
            moneyZeikomi: '999',
            rate: '10',
            checked: true
          },
          {
            foodName: '06 水煮牛肉',
            num: '1',
            moneyZeikomi: '999',
            rate: '10',
            checked: true
          },
          {
            foodName: '06 水煮牛肉',
            num: '1',
            moneyZeikomi: '999',
            rate: '10',
            checked: true
          },
          {
            foodName: '06 水煮牛肉',
            num: '1',
            moneyZeikomi: '999',
            rate: '10',
            checked: true
          }
        ],
        tableDataChecked: [
          {
            foodNameChecked: '06 水煮牛肉水煮牛肉100',
            numChecked: '2',
            moneyZeikomiChecked: '2340',
            rateChecked: '10'
          },
          {
            foodNameChecked: '06 水煮牛肉',
            numChecked: '2',
            moneyZeikomiChecked: '2340',
            rateChecked: '10'
          },
          {
            foodNameChecked: '06 水煮牛肉',
            numChecked: '2',
            moneyZeikomiChecked: '2340',
            rateChecked: '10'
          }
        ],
        url: require('../../assets/images/payType.png')
      }
    }
  },

  methods: {}
}
</script>
<style lang="scss" scope>
.el-button--mini {
  padding: 5px 10px;
  width: 46px;
}
//一覧行の高さを低くする
.el-table--medium th,
.el-table--medium td {
  padding: 0px 0 !important;
}
// 枠線を外す
.el-table th.is-leaf,
.el-table td {
  border-bottom: none;
}
//合計行の様式
.sumRow {
  font-size: 14px;
  height: 22px;
  color: rgb(96, 98, 102);
  font-family: 'Helvetica Neue', Helvetica, 'PingFang SC', 'Hiragino Sans GB', 'Microsoft YaHei', Arial,
    sans-serif Helvetica Neue, Helvetica, PingFang SC, Hiragino Sans GB, Microsoft YaHei, Arial, sans-serif body;
}
</style>
