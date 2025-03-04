<template>
  <div ref="sellineName" @click="closeSel">
    <van-sticky :offset-top="77">
      <van-row class="txt-desc">受付一覧</van-row>
    </van-sticky>

    <div class="recepetion-List">
      <ul
        v-for="(item, index) in recepetionList"
        :key="index"
        ref="listItem"
        class="select-reception"
        :class="item.listitemChoose === true ? 'selected-reception' : ''"
        :aa="'listItem' + index"
        @click.stop="chooseRecCheck($event, index, item)"
      >
        <li>
          <van-row>
            <van-col span="13">
              <span style="color: #c1c0c2">受付番号</span>
            </van-col>
            <van-col span="11">
              <span>{{ item.receptionNumber }}</span>
            </van-col>
          </van-row>
        </li>
        <li>
          <van-row>
            <van-col span="13">
              <span style="color: #c1c0c2; margin-left: 15%">人数</span>
            </van-col>
            <van-col span="6">
              <span>{{
                item.seatPeopleMan +
                item.seatPeopleWoman +
                item.seatPeopleChildren
              }}</span>
            </van-col>
          </van-row>
        </li>
        <li>
          <van-row>
            <van-col span="13">
              <span style="color: #c1c0c2">座席</span>
            </van-col>
            <van-col span="11">
              <span>{{ item.seatName }}</span>
            </van-col>
          </van-row>
        </li>
      </ul>
    </div>
    <div class="select-reception_btn">
      <van-col class="btn">
        <van-button
          :disabled="listitemChoose"
          class="btn-linear"
          @click.stop="changeSeat()"
        >
          変更
        </van-button>
      </van-col>
      <van-col class="btn">
        <van-button
          class="btn-linear"
          :disabled="listitemChoose"
          @click.stop="toCommoditySearchMethod()"
        >
          注文
        </van-button>
      </van-col>
      <van-col class="btn">
        <van-button
          class="btn-linear"
          :disabled="!listitemChoose"
          @click.stop="NewReception()"
        >
          新規受付
        </van-button>
      </van-col>
    </div>

    <!-- 座席選択ポップアップ -->
    <van-popup v-model="showDialog" class="order-remarks">
      <van-row class="title">
        {{ popupName }}
      </van-row>
      <div class="seatList">
        <van-checkbox-group v-model="result">
          <van-cell-group>
            <van-cell
              v-for="(item, index) in seatList"
              :key="index"
              clickable
              :title="`${item.seatName}`"
              @click.stop="chooseSeatCheck($event, index, item)"
            >
              <template #right-icon>
                <van-checkbox
                  ref="checkboxes"
                  v-model="item.selectedSeatFlg"
                  :name="item"
                  shape="square"
                />
              </template>
            </van-cell>
          </van-cell-group>
        </van-checkbox-group>
      </div>
      <van-row class="row-btn">
        <van-col span="12">
          <van-button type="default" class="cancel fl" @click.stop="cancel"
            >キャンセル</van-button
          >
        </van-col>
        <van-col span="12">
          <van-button
            type="primary"
            :disabled="this.result.length === 0"
            class="confirm fr"
            @click.stop="confirm"
            >OK</van-button
          ></van-col
        >
      </van-row>
      <van-cell
        v-if="showPack"
        class="seatCell"
        title="持ち帰り"
        @click.stop="pack"
      />
    </van-popup>

    <!-- 人数確認ポップアップ -->
    <van-popup v-model="showDialog_OK" class="order-remarks">
      <van-form ref="form">
        <van-row class="title"> 人数 </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>男：</span>
          </van-col>
          <van-col span="8">
            <van-field v-model="manNumber" type="digit" :maxlength="2" />
          </van-col>
          <van-col span="8">
            <span style="text-align: left; margin-left: 15%">名</span>
          </van-col>
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>女：</span>
          </van-col>
          <van-col span="8">
            <van-field v-model="womanNumber" type="digit" :maxlength="2" />
          </van-col>
          <van-col span="8">
            <span style="text-align: left; margin-left: 15%">名</span>
          </van-col>
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>子供：</span>
          </van-col>
          <van-col span="8">
            <van-field v-model="childrenNumber" type="digit" :maxlength="2" />
          </van-col>
          <van-col span="8">
            <span style="text-align: left; margin-left: 15%">名</span>
          </van-col>
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>人数：</span>
          </van-col>
          <van-col span="8">
            <span style="text-align: left">{{ sumPeopleNum }}名</span>
          </van-col>
        </van-row>
        <van-row class="select-down">
          <van-row class="select-input">
            <input v-model="value" type="text" @click.stop="searchInfo" />
            <i class="fa fa-caret-down" aria-hidden="true" />
          </van-row>

          <ul ref="selectList">
            <!-- 注意！注意！注意！这里循环遍历的是items，不再是data里的list数组 -->
            <li
              v-for="(item, index) in items"
              :key="index"
              @click.stop="selectCurrent(index, $event)"
            >
              <span>{{ item.text }}</span>
            </li>
          </ul>
        </van-row>

        <van-row class="row-btn">
          <van-button
            type="default"
            class="cancel fl"
            @click.stop="savePeopleCount()"
            >OK</van-button
          >
        </van-row>
      </van-form>
    </van-popup>
  </div>
</template>

<script>
import {
  getList,
  insertReceptionInsideData,
  selectZasekiChoiceData,
} from "@/api/cma/cma081";
import { clearShoppingCart } from "@/utils/auth";
import { searchKuBun } from "@/api/common";
export default {
  name: "ReceptionSelection",
  // 初期表示データ
  data() {
    const change = (val) => {
      return this.manNumber && this.womanNumber && this.childrenNumber;
    };
    return {
      change,
      // 控制弹窗显示
      showDialog: false,
      showDialog_OK: false,
      //持ち帰り
      showPack: false,
      option: [],
      // 受付リスト
      recepetionList: [],
      // 選択した受付
      checkReception: {},
      listitemChoose: true,
      popupName: "",
      seatList: [],
      seatList_Zenkai: [],
      seatList_temp: [],
      selectedSeatList: [],
      orderReception:[],
      result: [
        // 选择坐席Flag
        // selectedSeatFlg:'0',
        // 组设置模式
        // groupMode
      ],
      queryParam: {
        // 坐席选择mode
        seatSelectMode: "0",
        // 受付区分
        receptionKbn: "001",
      },
      // 控制打包活性
      controPackClick: true,
      showPeopleSize: false,
      peopleSize: "",
      seatPeopleMan: 0,
      seatPeopleWoman: 0,
      seatPeopleChildren: 0,
      isChange: false,
      // 人数ポップアップ内のダンロップ
      value: "",
      text: "",
      manNumber: "",
      womanNumber: "",
      childrenNumber: "",
    };
  },
  computed: {
    sumPeopleNum: function () {
      return (
        parseInt(this.manNumber ? this.manNumber : 0) +
        parseInt(this.womanNumber ? this.womanNumber : 0) +
        parseInt(this.childrenNumber ? this.childrenNumber : 0)
      );
    },
    // 过滤方法
    items: function () {
      var _search = this.value;
      if (_search) {
        // 不区分大小写处理
        var reg = new RegExp(_search, "ig");
        // es6 filter过滤匹配，有则返回当前，无则返回所有
        return this.option.filter(function (e) {
          // 匹配所有字段
          return Object.keys(e).some(function (key) {
            return e[key].match(reg);
          });
          // 匹配某个字段
          //  return e.name.match(reg);
        });
      }
      return this.option;
    },
  },
  mounted() {
    // 他の場所をクリックすると、BOXが閉じる
    document.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.show = false;
      }
    });
  },
  // 初期表示
  created() {
    this.getList();
    this.searchKuBun();
  },
  //方法
  methods: {
    getList() {
      getList().then((response) => {
        if (response.status === 200) {
          var seatList = [];
          var receptionList_temp = [];
          if (response.seatList.length === 0) {
            this.$msgUtil.messageNew("E-00010", "受付一覧", this);
            return;
          }
          for (let i = 0; i < response.seatList.length; i++) {
            if (response.seatList[i].seatStatusKbn === "002") {
              seatList.push(response.seatList[i]);
            }
            if (
              response.seatList[i].seatStatusKbn === "002" &&
              response.seatList[i].receptionBranchNumber === 1
            ) {
              receptionList_temp.push(response.seatList[i]);
              this.orderReception.push(JSON.parse(JSON.stringify(response.seatList[i])));
            }
            if (
              response.seatList[i].receptionNumber ===
              this.checkReception.receptionNumber
            ) {
              this.result.push(response.seatList[i]);
            }
          }

          receptionList_temp.forEach((item1) => {
            seatList.forEach((item) => {
              if (
                item.receptionNumber === item1.receptionNumber &&
                item.receptionBranchNumber !== 1
              ) {
                item1.seatName += " " + item.seatName;
              }
              item1.listitemChoose = false;
            });
          });
          this.recepetionList = receptionList_temp;
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        }
      });
    },
    chooseRecCheck($event, index, item) {
      const thisObj = this;
      thisObj.checkReception = item;
      this.result.push(this.orderReception[index]);
      this.result[0].selectedSeatFlg = true;
      if (item.listitemChoose === true) {
        item.listitemChoose = false;
        this.listitemChoose = true;
      } else {
        this.changeColor();
        item.listitemChoose = true;
        this.listitemChoose = false;
      }
    },
    // 変更ボタン
    changeSeat() {
      this.isChange = true; // 変更
      this.showPack = false; // 持ち帰りボタン
      this.seatList = [];
      this.result = [];
      this.seatList_Zenkai = [];
      this.popupName = "座席変更";
      //　ポップアップが表示する
      this.showDialog = true;
      this.GetChangeReceptionList();
    },
    GetChangeReceptionList() {
      getList().then((response) => {
        if (response.status === 200) {
          var seatBlockList = [];
          for (let i = 0; i < response.seatList.length; i++) {
            if (
              response.seatList[i].receptionNumber ===
              this.checkReception.receptionNumber
            ) {
              response.seatList[i].selectedSeatFlg = true;
              this.result.push(response.seatList[i]);
              this.seatList_Zenkai.push(response.seatList[i]);
            }
            if (response.seatList[i].seatStatusKbn === "001") {
              response.seatList[i].selectedSeatFlg = false;
              seatBlockList.push(response.seatList[i]);
            }
          }
          this.seatList_Zenkai.push.apply(this.seatList_Zenkai, seatBlockList);
          this.seatList = this.seatList_Zenkai;
        }
        // thisObj.$refs.checkboxes[index].toggle()
        else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        }
      });
    },
    // 新規受付
    NewReception() {
      this.isChange = false;
      this.showPack = true;
      this.seatList = [];
      this.result = [];
      var seatList_temp = [];
      this.controPackClick = true;
      this.popupName = "新規受付";
      //　ポップアップが表示する
      this.showDialog = true;

      getList().then((response) => {
        if (response.status === 200) {
          for (let i = 0; i < response.seatList.length; i++) {
            if (response.seatList[i].seatStatusKbn === "001") {
              response.seatList[i].selectedSeatFlg = false;
              seatList_temp.push(response.seatList[i]);
            }
          }
          this.seatList = seatList_temp;
        }
        // thisObj.$refs.checkboxes[index].toggle()
        else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        }
      });
    },
    searchKuBun() {
      searchKuBun({ categoryClassNumber: "070" }).then((response) => {
        for (let i = 0; i < response.categoryList.length; i++) {
          const item = {};
          item.value = response.categoryList[i].categoryKbn;
          item.text = response.categoryList[i].categoryKbnName;
          this.option.push(item);
        }
      });
    },
    // 持ち帰りボタン
    pack() {
      // 默认打包活性
      if (this.controPackClick) {
        // 活性时的处理
        // 放入缓存
        this.queryParam.receptionKbn = "002";

        this.saveSelectToStore();

        this.$router.push({
          path: "/Employee/packageAcceptance",
        });
      }
    },
    // 座席選択
    chooseSeatCheck($event, index, item) {
      const thisObj = this;

      thisObj.$refs.checkboxes[index].toggle();

      selectZasekiChoiceData({ seatNumber: item.seatNumber }).then(
        (response) => {
          if (response.status === 200) {
            thisObj.seatList[index].selectedSeatFlg =
              thisObj.$refs.checkboxes[index].checked;

            if (response.seatList[0].seatStatusKbn === "002" && thisObj.seatList[0].receptionNumber !== response.seatList[0].receptionNumber) {
              this.$msgUtil
                .messageBox("E_00070", "", this)
                .then(() => {
                  if (!thisObj.isChange) {
                    thisObj.NewReception();
                  } else {
                    thisObj.changeSeat();
                  }
                  return;
                })
                .catch(() => {
                  return;
                });
            }
            // 選択中座席有無
            let hasSeat = false;
            // 全て'001'（空）
            let allEmpty = true;
            for (let i = 0; i < thisObj.seatList.length; i++) {
              if (thisObj.seatList[i].selectedSeatFlg) {
                hasSeat = true;
                if (thisObj.seatList[i].seatStatusKbn !== "001") {
                  allEmpty = false;
                }
              }
            }

            if (hasSeat && allEmpty) {
              // '1'（新規）を設定
              thisObj.queryParam.seatSelectMode = "1";
              thisObj.controPackClick = false;
            } else if (hasSeat) {
              // '2'（追加）を設定
              thisObj.queryParam.seatSelectMode = "2";
              thisObj.controPackClick = false;
            } else {
              // '0'（選択なし）を設定
              thisObj.queryParam.seatSelectMode = "0";
              thisObj.controPackClick = true;
            }

            // 人数と人数内訳を計算
            if (thisObj.queryParam.seatSelectMode === "1") {
              thisObj.showPeopleSize = false;
            } else if (thisObj.queryParam.seatSelectMode === "2") {
              thisObj.showPeopleSize = true;
              thisObj.peopleSize = 0;
              thisObj.seatPeopleMan = 0;
              thisObj.seatPeopleWoman = 0;
              thisObj.seatPeopleChildren = 0;

              for (let i = 0; i < thisObj.seatList.length; i++) {
                if (thisObj.seatList[i].selectedSeatFlg) {
                  if (thisObj.seatList[i].groupMode === "1") {
                    thisObj.peopleSize =
                      thisObj.seatList[i].seatPeopleMan +
                      thisObj.seatList[i].seatPeopleChildren +
                      thisObj.seatList[i].seatPeopleWoman;

                    thisObj.seatPeopleMan = thisObj.seatList[i].seatPeopleMan;

                    thisObj.seatPeopleWoman =
                      thisObj.seatList[i].seatPeopleWoman;

                    thisObj.seatPeopleChildren =
                      thisObj.seatList[i].seatPeopleChildren;
                  }
                }
              }
            } else if (thisObj.queryParam.seatSelectMode === "0") {
              thisObj.showPeopleSize = false;
            }
            // thisObj.$refs.checkboxes[index].toggle()
          } else if (response.status === 601) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
          } else if (response.status === 602) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
          }
        }
      );
    },
    // 将人数输入框中的数据赋值到桌号最小的桌子
    savePeopleCount() {
      if (
        (this.manNumber === "" ||
          this.manNumber === "0" ||
          this.manNumber === 0) &&
        (this.womanNumber === "" ||
          this.womanNumber === "0" ||
          this.womanNumber === 0) &&
        (this.childrenNumber === "" ||
          this.childrenNumber === "0" ||
          this.childrenNumber === 0)
      ) {
        this.$msgUtil.messageNew("W_00070", null, this).then(() => {
          this.savePeopleCountOk();
        });
        return;
      } else {
        this.savePeopleCountOk();
      }
    },
    savePeopleCountOk() {
      this.$refs["form"].validate().then(() => {
        var minNumber = this.mathMin(this.result);
        this.result[minNumber].seatPeopleMan = this.manNumber;
        this.result[minNumber].seatPeopleWoman = this.womanNumber;
        this.result[minNumber].seatPeopleChildren = this.childrenNumber;
        this.result[minNumber].receptionRemarks = this.value;
        this.showDialog = false;
        this.showDialog_OK = false;

        this.InsertChangeReception();
      });
    },
    // 登録処理
    InsertChangeReception() {
      const thisObj = this;
      this.seatList_temp = [];
      this.selectedSeatList = [];

      for (let i = 0; i < this.result.length; i++) {
        if (this.result[i].selectedSeatFlg) {
          var selectedSeats = {
            selectedSeatNumber: this.result[i].seatNumber,
          };
          this.selectedSeatList.push(selectedSeats);
        }
        if (
          this.result[i].seatPeopleMan === "" ||
          this.result[i].seatPeopleMan === null
        ) {
          this.result[i].seatPeopleMan = "0";
        }
        if (
          this.result[i].seatPeopleWoman === "" ||
          this.result[i].seatPeopleWoman === null
        ) {
          this.result[i].seatPeopleWoman = "0";
        }
        if (
          this.result[i].seatPeopleChildren === "" ||
          this.result[i].seatPeopleChildren === null
        ) {
          this.result[i].seatPeopleChildren = "0";
        }
        this.seatList_temp.push({
          seatLevel: this.result[i].seatLevel,
          seatNumber: this.result[i].seatNumber,
          receptionNumber: this.result[i].receptionNumber,
          receptionBranchNumber: this.result[i].receptionBranchNumber,
          seatStatusKbn: this.result[i].seatStatusKbn,
          seatPeopleMan: this.result[i].seatPeopleMan,
          seatPeopleWoman: this.result[i].seatPeopleWoman,
          seatPeopleChildren: this.result[i].seatPeopleChildren,
          receptionRemarks: this.result[i].receptionRemarks,
          selectedSeatFlg: this.result[i].selectedSeatFlg === true ? "1" : "0",
          groupMode: this.result[i].groupMode,
        });
      }
      // 設計書通り、渡すパラメータを構築
      var submitData = {
        selectedSeatList: this.selectedSeatList,
        selectedSeatOnoff: this.result.length > 0 ? "1" : "0",
        seatSelectMode: this.queryParam.seatSelectMode,
        menuLineNumber: "",
        menuOrderNumber: "",
        receptionKbn: this.queryParam.receptionKbn,
        seatList: this.seatList_temp,
      };

      insertReceptionInsideData(submitData).then((response) => {
        if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this);
        } else if (response.status === 200) {
          let args = "";
          for (let i = 0; i < thisObj.result.length; i++) {
            if (i === thisObj.result.length - 1) {
              args += thisObj.result[i].seatName;
            } else {
              args += thisObj.result[i].seatName + ";";
            }
          }
          thisObj.$msgUtil.messageBox("I_00010", args).then(() => {
            this.saveSelectToStore();
            thisObj.$router.push({
              path: "/Employee/CommoditySearchMethod",
            });
          });
        }
      });
    },
    //　OKボタン
    confirm() {
      this.showDialog_OK = true;
      this.manNumber = "";
      this.womanNumber = "";
      this.childrenNumber = "";
      this.value = "";

      if (this.isChange) {
        if (this.seatList_Zenkai.length !== 0) {
          this.manNumber = this.seatList_Zenkai[0].seatPeopleMan;
          this.womanNumber = this.seatList_Zenkai[0].seatPeopleWoman;
          this.childrenNumber = this.seatList_Zenkai[0].seatPeopleChildren;
          this.value = this.seatList_Zenkai[0].receptionRemarks;
        }
      }
    },
    // キャンセル
    cancel() {
      this.showDialog = false;
      this.showDialog_OK = false;
    },
    // 选中当前检索项
    selectCurrent(index, event) {
      if (this.showDialog_OK) {
        var menuText = event.currentTarget.innerText;
        this.value = menuText;

        this.$refs.selectList.style.display = "none";
      }
    },
    changeColor() {
      for (let i = 0; i < this.recepetionList.length; i++) {
        this.recepetionList[i].listitemChoose = false;
      }
    },
    // 注文
    toCommoditySearchMethod() {
      const thisObj = this;
      this.saveSelectToStore();
      thisObj.$router.push({
        path: "/Employee/CommoditySearchMethod",
      });
    },
    // session保存
    saveSelectToStore() {
      if (this.result.length !== 0) {
        var minIndex = this.mathMin(this.result);
      }
      sessionStorage.setItem(
        "seatInfo",
        JSON.stringify({
          selectedSeats: this.result,
          receptionKbn: this.queryParam.receptionKbn,
          mainSeat: this.result.length === 0 ? null : this.result[minIndex],
          seatName:
            this.result.length === 0 ? "" : this.result[minIndex].seatName,
          seatNumber:
            this.result.length === 0 ? "" : this.result[minIndex].seatNumber,
        })
      );
    },
    // 输入框输入事件
    searchInfo() {
      if (this.showDialog_OK) {
        this.$refs.selectList.style.display = "block";
      }
    },
    // 查看最小的桌号
    mathMin(arrs) {
      if (arrs.length === 0) {
        return null;
      }
      var minNumberSeatIndex = null;
      for (var i = 0; i < arrs.length; i++) {
        if (arrs[i].selectedSeatFlg) {
          if (minNumberSeatIndex === null) {
            minNumberSeatIndex = i;
          }
          if (arrs[i].seatNumber < arrs[minNumberSeatIndex].seatNumber) {
            minNumberSeatIndex = i;
          }
        }
      }
      return minNumberSeatIndex;
    },
    closeSel(event) {
      const currentCli = this.$refs.sellineName;
      if (currentCli && this.showDialog) {
        // if (!currentCli.contains(event.target)) {
        // 点击到了id为sellineName以外的区域，隐藏下拉框
        this.$refs.selectList.style.display = "none";
        // }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/styles/variables.scss";

.txt-desc {
  padding-left: 40px;
}

.list-info {
  padding-bottom: 150px;
}

// 人数
.select-seat-wrap {
  height: 100px;
}
.select-reception_btn {
  width: 100%;
  height: 100px;
  background-color: $mainColor;
  position: fixed;
  margin-top: 100px;
  bottom: 0;
  left: 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 25px 0 40px;
  box-sizing: border-box;
}
.select-reception {
  padding: 10px 20px;
  overflow: hidden;
  border-top: $orderLinear1 solid 0.5px;
  background-color: rgba($color: $mainColor, $alpha: 0.2);
  // background: linear-gradient(to right, $titleBgColorLinear1, $titleBgColorLinear2 100%);
  border-radius: 5px;
  margin-bottom: 0px;
  font-size: $smallSize;
}
.selected-reception {
  padding: 10px 20px;
  overflow: hidden;
  border-top: $switchColor solid 0.5px;
  border-bottom: $switchColor solid 0.5px;
  background-color: rgba($color: $yellow, $alpha: 0.05);
  // background: linear-gradient(to right, $titleBgColorLinear1, $titleBgColorLinear2 100%);
  border-radius: 5px;
  margin-bottom: 0px;
  font-size: $smallSize;
}
.btn {
  width: 60%;
  border-radius: 10px;
  border: 0 !important;
  height: 40px;
  border-radius: 5px;
}
.recepetion-List {
  margin-top: 0px;
  margin-bottom: 100px;

  li {
    width: 50%;
    float: left;
    line-height: 35px;
    margin-bottom: 0px;
  }
}

.datelistStyle {
  position: relative;
  .van-icon {
    transform: rotate(90deg);
    position: absolute;
    right: 10px;
    top: 15px;
  }

  input {
    width: 100%;
    height: 40px;
    border-radius: 5px;
    background-color: #1c1a21;
    border: 1px solid #ced4da;
    padding-left: 5px;
  }
}

input::-webkit-calendar-picker-indicator {
  background-color: inherit;
}

.seatList {
  .van-cell {
    font-size: 14px;
  }
}
</style>

<style lang="scss">
@import "@/styles/variables.scss";
.order-remarks {
  .van-field__control {
    color: $white !important;
  }

  .van-dropdown-item__content {
    height: 95px;
  }

  .input-box {
    .van-field__control {
      text-align: right;
    }
  }
}
</style>
<style lang="scss" scoped>
@import "@/styles/variables.scss";

.order-remarks {
  max-height: 80%;
  .input-box {
    margin: 0 auto;

    span {
      display: inline-block;
      line-height: 38px;
      width: 50px;
      margin-bottom: 15px;
      text-align: right;
    }

    input {
      width: 65px;
      height: 40px;
      border: 1px solid rgba($color: $white, $alpha: 0.6);
      margin-right: 10px;
      background-color: transparent;
      padding: 0 5px;
      box-sizing: border-box;
    }

    .van-cell {
      padding: 6px 10px;
      border: 1px solid #7d7d7f !important;
    }
  }
  .van-overlay {
    height: auto;
  }

  .row-btn {
    justify-content: center;

    button {
      width: 105px;
      height: 40px;
      line-height: 40px;
      color: $white;
      border: 0;
      border-radius: 5px;
    }

    div:first-child button {
      background: $inputBorder;
    }

    div:last-child button {
      background-image: linear-gradient(
        to right,
        $linearBorder2,
        $linearBorder1
      ) !important;
    }
  }

  .remarks-info {
    .radio-item {
      display: flex;
      align-items: center;
    }
  }
}
</style>