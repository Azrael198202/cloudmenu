module.exports = [
    {
        url: '/order/searchNyukoProduct.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                searchList: [
                    {
                        categoryName: '鶏肉もも商品',
                        categoryKbn: '102'
                    },
                    {
                        categoryName: '鶏肉むね商品',
                        categoryKbn: '103'
                    }
                ],
                msgList: [{ msgCode: 'E_00010', msgOption: '検索でエラーが発生しました。', msgItemId: '' }]
            }
        }
    },
    {
        url: '/order/searchNyukoMaterial.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                searchList: [
                    {
                        categoryName: '鶏肉もも原料',
                        categoryKbn: '102'
                    },
                    {
                        categoryName: '鶏肉むね原料',
                        categoryKbn: '103'
                    }
                ],
                msgList: [{ msgCode: 'E_00010', msgOption: '検索でエラーが発生しました。', msgItemId: '' }]
            }
        }
    },
    {
        url: '/order/searchNyukoEquipment.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                searchList: [
                    {
                        categoryName: '鶏肉もも備品',
                        categoryKbn: '102'
                    },
                    {
                        categoryName: '鶏肉むね備品',
                        categoryKbn: '103'
                    }
                ],
                msgList: [{ msgCode: 'E_00010', msgOption: '検索でエラーが発生しました。', msgItemId: '' }]
            }
        }
    }
]
