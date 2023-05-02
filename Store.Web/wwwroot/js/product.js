var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        columns: [
            { data: 'title', "width": "25%" },
            { data: 'author', "width": "20%" },
            { data: 'category.name', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-100 pt-2 btn-group" role="group">
                            <a href="/Admin/Product/Upsert/${data}" class="btn btn-dark mx-2" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        console.log(result)
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })

}