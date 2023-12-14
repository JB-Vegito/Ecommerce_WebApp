var dataTable;

$(document).ready(function(){
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#dataTable').DataTable({
        "ajax":{
            "url":"/cart/GetCart"
        },
        "columns":[
            {"data":"inventory.name","width":"15%"},
            {"data":"item_Quantity","width":"2%"},           
        ]
    });
}