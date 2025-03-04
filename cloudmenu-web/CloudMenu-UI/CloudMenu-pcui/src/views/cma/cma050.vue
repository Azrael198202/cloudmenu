<template>
  <div class="container">
    <div class="main">
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>店舗名</span></el-col>
        <el-col :span="4"><el-input v-model="form.monoName"></el-input></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>住所</span></el-col>
        <el-col :span="4"><el-input v-model="form.monoName"></el-input></el-col>
        <el-col :span="3" :offset="5"><span>電話番号</span></el-col>
        <el-col :span="4"><el-input v-model="form.monoName"></el-input></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>ホームページ</span></el-col>
        <el-col :span="4"><el-input v-model="form.monoName"></el-input></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>営業時間</span></el-col>
        <el-col :span="2">平日</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value1" format="HH:MM"></el-time-picker>
        </el-col>
        <el-col :span="1" :offset="1" class="text-center">～</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value2" format="HH:MM"></el-time-picker>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"></el-col>
        <el-col :span="2">土曜日</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value1" format="HH:MM"></el-time-picker>
        </el-col>
        <el-col :span="1" :offset="1" class="text-center">～</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value2" format="HH:MM"></el-time-picker>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"></el-col>
        <el-col :span="2">日・祝</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value1" format="HH:MM"></el-time-picker>
        </el-col>
        <el-col :span="1" :offset="1" class="text-center">～</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value2" format="HH:MM"></el-time-picker>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="12"><span>座席数</span></el-col>
        <el-col :span="12"><span>提供ジャンル</span></el-col>
      </el-row>
      <el-row type="flex">
        <el-col :span="12">
          <el-table :data="form.tableData" ref="multipleTable" :header-cell-style="rowClass" style="width:391px">
            <el-table-column type="selection" prop="selectFlg" width="40"></el-table-column>
            <el-table-column label="種類" prop="type" width="100"></el-table-column>
            <el-table-column label="数量" align="center">
              <el-table-column label="" prop="numberTable" show-overflow-tooltip width="80"></el-table-column>
              <el-table-column label="" prop="unitTable" show-overflow-tooltip width="45"></el-table-column>
            </el-table-column>
            <el-table-column label="定員" align="center">
              <el-table-column label="" prop="numberPeople" show-overflow-tooltip width="80"></el-table-column>
              <el-table-column label="" prop="unitPeople" show-overflow-tooltip width="45"></el-table-column>
            </el-table-column>
          </el-table>
        </el-col>
        <el-col :span="12">
          <el-table :data="form.tableDataShanru" border ref="multipleTable" style="width:141px">
            <el-table-column type="selection" prop="selectFlg" width="40"></el-table-column>
            <el-table-column label="種類" prop="type" width="100" align="center"></el-table-column>
          </el-table>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="12"><span>注文一覧画面</span></el-col>
        <el-col :span="12"><span>アレルギー食材</span></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4"><span>リフレッシュサイクル</span></el-col>
        <el-col :span="4"><el-input v-model="form.refreshCircle"></el-input></el-col>
        <el-col :span="12" :offset="4">
          <el-radio v-model="form.allergyViewFlg" label="1">メニューに表示する</el-radio>
          <el-radio v-model="form.allergyViewFlg" label="2">メニューに表示しない</el-radio>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>店舗ロゴ</span></el-col>
        <el-col :span="3" :offset="3"><span>お店の紹介</span></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="1">
          <div class="block">
            <el-image style="width: 100px; height: 100px" :src="form.url" :preview-src-list="form.srcList">
              <div slot="error" class="el-image__error"></div>
            </el-image>
          </div>
        </el-col>
        <el-col :span="17" :offset="2">
          <el-input type="textarea" v-model="form.textarea" rows="5" resize="none"></el-input>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="1">
          <el-button class="red-btn" plain>参照</el-button>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>店舗写真</span></el-col>
        <el-col :span="3" :offset="3"><span>オーナー一言</span></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="1">
          <div class="block">
            <el-image style="width: 100px; height: 100px" :src="form.url" :preview-src-list="form.srcList">
              <div slot="error" class="el-image__error"></div>
            </el-image>
          </div>
        </el-col>
        <el-col :span="17" :offset="2">
          <el-input type="textarea" v-model="form.textarea" rows="5" resize="none"></el-input>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="1">
          <el-button class="red-btn" plain>参照</el-button>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="2" :offset="20">
          <el-button class="red-btn" plain>QR</el-button>
        </el-col>
        <el-col :span="2">
          <el-button class="red-btn" plain>登録</el-button>
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
        tableData: [
          {
            selectFlg: true,
            type: 'テーブル',
            numberTable: '10',
            unitTable: '卓',
            numberPeople: '40',
            unitPeople: '人'
          },
          {
            selectFlg: true,
            type: 'カウンター',
            numberTable: '10',
            unitTable: '席',
            numberPeople: '10',
            unitPeople: '人'
          },
          {
            selectFlg: true,
            type: 'カウンター',
            numberTable: '5',
            unitTable: '卓',
            numberPeople: '30',
            unitPeople: '人'
          },
          {
            selectFlg: true,
            type: '座敷',
            numberTable: '10',
            unitTable: '卓',
            numberPeople: '6',
            unitPeople: '人'
          },
          {
            selectFlg: true,
            type: '広間',
            numberTable: '10',
            unitTable: '卓',
            numberPeople: '6',
            unitPeople: '人'
          }
        ],
        tableDataShanru: [
          {
            selectFlg: true,
            type: '中華料理'
          },
          {
            selectFlg: true,
            type: '和食'
          },
          {
            selectFlg: true,
            type: '定食'
          },
          {
            selectFlg: true,
            type: 'アジア・エスニック'
          }
        ],
        multipleSelection: [],
        url: require('../../assets/images/cma040.jpg'),
        srcList: [require('../../assets/images/cma040.jpg')]
      }
    }
  },
  methods: {
    rowClass({ row, column, rowIndex, columnIndex }) {
      if (rowIndex === 0 && (columnIndex === 2 || columnIndex === 3)) {
        this.$nextTick(() => {
          if (document.getElementsByClassName(column.id).length !== 0) {
            document.getElementsByClassName(column.id)[0].setAttribute('rowSpan', 2)
            return false
          }
        })
      }
      if (rowIndex === 1 && (columnIndex === 0 || columnIndex === 1)) {
        return { display: 'none' }
      }
    }
  }
}
</script>
<style>
span {
  font-size: 14px;
}
.el-row {
  margin-top: 10px !important;
}
/* 时间控件宽度 */
.el-date-editor--time {
  width: 160px !important;
}
</style>
