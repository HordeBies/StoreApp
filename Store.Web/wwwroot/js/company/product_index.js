var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/company/product/getallcompanyproducts' },
        columns: [
            { data: 'product.title', "width": "25%" },
            { data: 'price', "width": "20%" },
            { data: 'listPrice', "width": "15%" },
            {
                data: 'productId',
                "render": function (data) {
                    return `
                        <div class="w-100 pt-2 btn-group" role="group">
                            <a href="/Company/Product/Upsert/${data}" class="btn btn-dark mx-2" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick=Delete("/Company/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ]
    });
}