var dataTable;

$(document).ready(function(){
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#dataTable').DataTable({
        "ajax":{
            "url":"/product/GetAll"
        },
        "columns":[
            {"data":"id","width":"5%"},
            {"data":"name","width":"15%"},
            {"data":"description","width":"35%"},
            {"data":"category.categoryName","width":"10%"},
            {"data":"price","width":"10%"},
            {"data":"quantity","width":"5%"},
            {
                "data":"id",
                "render": function(data){
                    return `
                        <div class="pt-3">
                        <a href="/Product/AdminProductUpdate?id=${data}" style="text-decoration:none;">
                        <button class="btn btn-success p-2 ms-2 my-2"><i class="bi bi-wrench-adjustable-circle"></i> &nbsp; Update</button>
                        </a>  
                        <a href="/Product/AdminProductDelete?id=${data}" style="text-decoration:none;">
                        <button class="btn btn-primary p-2 ms-2 my-2"><i class="bi bi-trash3-fill"></i> &nbsp; Delete</button>
                        </a>    
                        </div>
                    `
                },
                "width":"10%"
            }
        ]
    });
}