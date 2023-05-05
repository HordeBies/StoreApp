var dataTable;
$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
        var status = urlParams.get('status');
        loadDataTable(status);
})
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: `/admin/order/getall?status=${status}` },
        columns: [
            { data: 'id', "width": "10%" },
            { data: 'fullName', "width": "20%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-100 pt-2 btn-group" role="group">
                            <a href="/Admin/Order/Details/${data}" class="btn btn-dark mx-2" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i> Details
                            </a>
                        </div>
                    `;
                },
                "width": "15%"
            }
        ]
    });
}

