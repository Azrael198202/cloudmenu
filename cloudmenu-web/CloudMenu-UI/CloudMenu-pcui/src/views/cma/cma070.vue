<template>
  <div class="container">
    <div class="main">
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>区分分類</span></el-col>
        <el-col :span="4">
          <el-select v-model="form.catalogBig">
            <el-option
              v-for="item in form.catalogBigList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
        <el-col :span="3" :offset="1"><span>区分分類名</span></el-col>
        <el-col :span="4"><el-input v-model="form.monoName"></el-input></el-col>
      </el-row>
      <el-row style="margin-top:20px">
        <el-col :span="24">
          <el-card>
            <div style="margin-top: 20px">
              <el-button>
                <i class="fa fa-plus" aria-hidden="true"></i>
                新規
              </el-button>
              <el-button>
                <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                修正
              </el-button>
              <el-button>
                <i class="fa fa-trash" aria-hidden="true"></i>
                削除
              </el-button>
            </div>
            <el-table :data="form.masterItiran" height="300">
              <el-table-column label="No" width="80">
                <template slot-scope="scope">
                  <span>{{ scope.row.index }}</span>
                </template>
              </el-table-column>
              <el-table-column label="区分分類" width="80">
                <template slot-scope="scope">
                  <span>{{ scope.row.kubunBunrui }}</span>
                </template>
              </el-table-column>
              <el-table-column label="区分コード" width="120">
                <template slot-scope="scope">
                  <p>{{ scope.row.kubunCode }}</p>
                </template>
              </el-table-column>
              <el-table-column label="区分名（設定値）" width="180">
                <template slot-scope="scope">
                  <p>{{ scope.row.kubunName }}</p>
                </template>
              </el-table-column>
              <el-table-column label="備考" width="220">
                <template slot-scope="scope">
                  <p>{{ scope.row.comment }}</p>
                </template>
              </el-table-column>
            </el-table>
          </el-card>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle" style="margin-top:20px">
        <el-col :span="3" :offset="21">
          <el-button type="danger">登録</el-button>
        </el-col>
      </el-row>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      form: {
        radio: '1',
        masterItiran: [
          {
            index: '1',
            kubunBunrui: '001',
            kubunCode: '1',
            kubunName: '単品',
            comment: ''
          },
          {
            index: '2',
            kubunBunrui: '001',
            kubunCode: '2',
            kubunName: '定食',
            comment: ''
          },
          {
            index: '1',
            kubunBunrui: '001',
            kubunCode: '1',
            kubunName: '肉料理',
            comment: ''
          },
          {
            index: '2',
            kubunBunrui: '001',
            kubunCode: '2',
            kubunName: '魚料理',
            comment: ''
          }
        ]
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
</style>
