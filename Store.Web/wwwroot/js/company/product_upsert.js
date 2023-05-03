var dataTable;
function loadProductDataTable() {
    $("#loadProductDataTableButton").hide();

    $("#tblDataParent").show();
    if (dataTable != null) {
        dataTable.ajax.reload();
        return;
    }
    
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/company/product/getallproducts' },
        columns: [
            { data: 'title', "width": "25%" },
            { data: 'author', "width": "20%" },
            { data: 'category.name', "width": "15%" },
            {data: 'isbn', "width":"10%"},
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-100 pt-2 btn-group" role="group">
                            <a href="/company/product/upsert/${data}" class="btn btn-success mx-2" style="cursor:pointer">
                                <i class="bi bi-check2-square"></i> Select
                            </a>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ]
    });
}