module.exports = [{
        url: '/zaiko/searchTana.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                searchList: [
                    { targetNumber: 1, targetName: '雞肉', lastDate: '05/17', lastQuantity: 14, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 14 },
                    { targetNumber: 2, targetName: '雞肉', lastDate: '05/17', lastQuantity: 14, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 14 },
                    { targetNumber: 3, targetName: '雞肉', lastDate: '05/18', lastQuantity: 14, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 14 }
                    // { targetNumber: 4, targetName: '雞肉', lastDate: '05/19', lastQuantity: 14, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 14 },
                    // { targetNumber: 5, targetName: '雞肉', lastDate: '05/20', lastQuantity: 16, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 16 },
                    // { targetNumber: 6, targetName: '雞肉', lastDate: '05/21', lastQuantity: 14, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 14 },
                    // { targetNumber: 7, targetName: '雞肉', lastDate: '05/22', lastQuantity: 14, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 14 },
                    // { targetNumber: 8, targetName: '雞肉', lastDate: '05/23', lastQuantity: 17, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 17 },
                    // { targetNumber: 9, targetName: '雞肉', lastDate: '05/24', lastQuantity: 18, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 18 },
                    // { targetNumber: 10, targetName: '雞肉', lastDate: '05/25', lastQuantity: 17, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 17 },
                    // { targetNumber: 11, targetName: '雞肉', lastDate: '05/26', lastQuantity: 17, storingQuantity: 5, consumptionQuantity: 5, stockQuantity: 17 }
                ],
                msgList: [
                    { msgCode: 'E_00010', msgItemId: '', msgOption: '棚卸検索' }
                ]
            }
        }
    },
    {
        url: '/zaiko/checkTana.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                msg: ''
            }
        }
    },
    {
        url: '/zaiko/saveTana.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                msgList: [
                    { msgCode: 'E_00080', msgItemId: '', msgOption: 'DBの処理に失敗しました。' }
                ]
            }
        }
    }
]