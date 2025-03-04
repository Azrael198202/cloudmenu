module.exports = [
    {
        url: '/order/addNyukoProduct.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                // status: 601,
                msgList: [{ msgCode: 'E_00080', msgOption: '', msgItemId: '' }]
            }
        }
    },
    {
        url: '/order/addNyukoMaterial.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                // status: 601,
                msgList: [{ msgCode: 'E_00080', msgOption: '', msgItemId: '' }]
            }
        }
    },
    {
        url: '/order/addNyukoEquipment.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                // status: 601,
                msgList: [{ msgCode: 'E_00080', msgOption: '', msgItemId: '' }]
            }
        }
    }
]
