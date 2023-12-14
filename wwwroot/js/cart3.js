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
            {"data":"cart_Id","width":"2%"},
            {"data":"inventory.name","width":"15%"},
            {"data":"item_Quantity","width":"2%"},
            {
                "data":"cart_Id",
                "render": function(data){
                    return `
                        <div class="pt-3">
                        <ul style="list-style-type:none; display:flex;">
                        <li>
                        <a href="/Cart/CartUpdateUp?id=${data}" style="text-decoration:none;">
                        <button class="btn btn-info btn-outline-light p-2 ms-2 my-2"><i class="bi bi-chevron-up"></i></button>
                        </a>  
                        </li>
                        <li>
                        <a href="/Cart/CartUpdateDown?id=${data}" style="text-decoration:none;">
                        <button class="btn btn-info btn-outline-light p-2 ms-2 my-2"><i class="bi bi-chevron-down"></i></button>
                        </a>
                        </li>
                        </ul>     
                        </div>
                    `
                },
                "width":"1%"
            },
            {
                "data":"cart_Id",
                "render": function(data){
                    return `
                        <div class="pt-3">
                        <a href="/Cart/CartDelete?id=${data}"><button type="submit" class="btn btn-danger btn-outline-dark p-2 ms-2 my-2"><i class="bi bi-trash3"></i></button></a> 
                        </div>
                    `
                },
                "width":"1%"
            }            
        ]
    });
}