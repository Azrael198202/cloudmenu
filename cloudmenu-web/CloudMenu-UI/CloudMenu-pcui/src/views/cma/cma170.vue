<template>
  <div class="container">
    <div class="main">
      <el-row type="flex" align="middle">
        <el-radio v-model="form.radio" label="1">材料コード</el-radio>
        <el-radio v-model="form.radio" label="2">材料名</el-radio>
        <el-col :span="3" :offset="2">
          <el-input>
            <i class="el-icon-search" slot="prefix"></i>
          </el-input>
        </el-col>
        <el-col :span="3" :offset="2">
          <el-button type="danger">搜索</el-button>
        </el-col>
        <el-col :span="3" :offset="6">
          <el-button type="danger">棚卸開始</el-button>
        </el-col>
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
            <el-table :data="form.zaikoItiran" height="300">
              <el-table-column type="index"></el-table-column>
              <el-table-column label="材料コード" width="100">
                <template slot-scope="scope">
                  <span>{{ scope.row.materialCode }}</span>
                </template>
              </el-table-column>
              <el-table-column label="材料名" width="250" align="center">
                <template slot-scope="scope">
                  <p>{{ scope.row.materialName }}</p>
                </template>
              </el-table-column>
              <el-table-column label="在庫数" width="180" align="center">
                <template slot-scope="scope">
                  <p>{{ scope.row.zaikoNum }} {{ scope.row.tani }}</p>
                </template>
              </el-table-column>
              <el-table-column label="前回棚卸数" width="120" align="center">
                <template slot-scope="scope">
                  <p>{{ scope.row.preTanaOrosiNum }} {{ scope.row.tani }}</p>
                </template>
              </el-table-column>
              <el-table-column label="入力値" width="120" align="center">
                <template slot-scope="scope">
                  <el-input v-model="scope.row.inputNum">
                    <i class="el-input__icon" slot="suffix">{{ scope.row.tani }}</i>
                  </el-input>
                </template>
              </el-table-column>
            </el-table>
            <el-pagination background layout="prev, pager, next" :total="1000"></el-pagination>
            <el-row>
              <el-col type="flex" align="middle">
                <el-button>
                  一時保存
                </el-button>
                <el-button>
                  棚卸反映
                </el-button>
                <el-button>
                  閉じる
                </el-button>
              </el-col>
            </el-row>
          </el-card>
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
        zaikoItiran: [
          {
            materialCode: '100100',
            materialName: '玉ねぎ',
            zaikoNum: '30',
            preTanaOrosiNum: '3',
            inputNum: '',
            tani: '個'
          },
          {
            materialCode: '100100',
            materialName: '玉ねぎ',
            zaikoNum: '30',
            preTanaOrosiNum: '3',
            inputNum: '',
            tani: '個'
          },
          {
            materialCode: '100100',
            materialName: '玉ねぎ',
            zaikoNum: '30',
            preTanaOrosiNum: '3',
            inputNum: '',
            tani: '個'
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
