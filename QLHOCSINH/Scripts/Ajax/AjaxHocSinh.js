
    $(document).ready(function () {
        LoadData();
        XuLyNhapLieuADD();
        XuLyNhapLieuEdit();
    $("#btnAdd").click(function () {
        AddHocSinh();
            });
    $(document).on("click", ".btnEdit", function () {
                var MaHS = $(this).closest("tr").find("td:eq(1)").text();
    GetById(MaHS);
            });
    $('#exampleModal').on('show.bs.modal', function (e) {
        TangID();
            });

    $("#btnSaveEdit").click(function () {
        UpdateHS();
    });
    $(document).on("click", ".btnDelete", function () {
                var id = $(this).closest("tr").find("td:eq(1)").text();
    var name = $(this).closest("tr").find("td:eq(2)").text();
    if (confirm("Bạn có chắc muốn xóa học sinh '" + name + "'không?")) {
        DeleteHocSinh(id);
                }
            });

    $('#exampleModal').on('hidden.bs.modal', function () {
        $('#quadratic-form input[type="text"]').val('');
    $('#quadratic-form input[type="date"]').val('');
    $('#quadratic-form select').val('');
    $('#quadratic-form input[type="checkbox"]').prop('checked', false);
            });

    $('#btnReset').click(function () {
        resetForm();
    });
    $('#btnExitEdit').click(function () {
        resetError();
    });
    $('#btnExit').click(function () {
        resetForm();
    });
       
   });

    function GetById(MaHS) {
        $.ajax({
            url: '/HocSinh/GetById',
            type: 'GET',
            data: { MaHS: MaHS },
            success: function (res) {
                $('#EditMaHS').val(res.data.MaHS);
                $('#EditHoTen').val(res.data.HoTen);
                $('#EditNgaySinh').val(res.data.NgaySinh);
                $('#EditGioiTinh').prop('checked', res.data.GioiTinh);
                $('#EditDiaChi').val(res.data.DiaChi);
                $('#EditMaLop').val(res.data.MaLop);
                $('#EditDiemTB').val(res.data.DiemTB);
            }
        });
};
function TangID() {
    $.ajax({
        url: '/HocSinh/GetMaxMaHS',
        type: 'GET',
        success: function (data) {
            var maxId = data.maxId;
            // Tăng ID lên 1 và gán vào trường MaHS
            var newId = "HS" + (maxId + 1);
            $('#MaHS').val(newId);
        }
    });
};
    function LoadData() {
        $.ajax({
            url: '/HocSinh/GetHocSinh',
            type: 'GET',
            success: function (res) {
                var items = res.data;
                var html = "";
                for (let i = 0; i < items.length; i++) {
                    var ngaySinh = new Date(parseInt(items[i].NgaySinh.substr(6)));
                    var formattedDate = ngaySinh.getDate() + '-' + (ngaySinh.getMonth() + 1) + '-' + ngaySinh.getFullYear();
                    var index = i + 1;
                    html += "<tr>";
                    html += "<td>" + index + "</td>"
                    html += "<td>" + items[i].MaHS + "</td>";
                    html += "<td>" + items[i].HoTen + "</td>";
                    html += "<td>" + formattedDate + "</td>";
                    html += "<td>" + items[i].GioiTinh + "</td>";
                    html += "<td>" + items[i].DiaChi + "</td>";
                    html += "<td>" + items[i].Lop + "</td>";
                    html += "<td>" + items[i].DiemTB + "</td>";
                    html += "<td><button class='btn btn-icon btn-hover btn-sm btn-rounded pull-right btnEdit' data-toggle='modal' data-target='#EditModal'> <i class='anticon anticon-edit'></i></button><button class='btn btn-icon btn-hover btn-sm btn-rounded pull-right btnDelete'> <i class='anticon anticon-delete'></i></button> </td> ";
                    html += "</tr>";
                }
                $('#showdata').html(html);
            }
        });
        };
    function AddHocSinh(){
            var MaHS = $('#MaHS').val().trim();
    var HoTen = $('#HoTen').val().trim();
    var NgaySinh = $('#NgaySinh').val();
    var GioiTinh = $('#GioiTinh').is(':checked');
    var DiaChi = $('#DiaChi').val().trim();
    var MaLop = $('#MaLop').val();
    var DiemTB = $('#DiemTB').val().trim();
        if (!HoTen) {
            $('#HoTenError').text('Họ và tên là trường bắt buộc');
            return;
        }
        else if (!NgaySinh) {
            $('#NgaySinhError').text('Ngày sinh là trường bắt buộc');
            return;
        }
        else if (!DiaChi) {
            $('#DiaChiError').text('Địa chỉ là trường bắt buộc');
            return;
        }
        else if (!DiemTB) {
            $('#DiemTBError').text('Điểm trung bình là trường bắt buộc');
            return;
        }
            
    $.ajax({
        type: 'POST',
    url: '/HocSinh/Add',
    dataType: 'JSON',
    contentType: "application/json; charset=utf-8",
    data: JSON.stringify({
        MaHS: MaHS,
    HoTen: HoTen,
    NgaySinh: NgaySinh,
    GioiTinh: GioiTinh,
    DiaChi: DiaChi,
    MaLop: MaLop,
    DiemTB: DiemTB
                }),
    success: function (response) {
        alert(response.status);
    LoadData();
                },
    error: function (response) {
        alert(response.status)
    },
            });
        };

    function UpdateHS() {
        var HoTen = $('#EditHoTen').val();
        if (HoTen.trim() === '') {
            $('#EditHoTenError').text('Họ và tên là trường bắt buộc');
            return;
        }
        var NgaySinh = $('#EditNgaySinh').val();
        if (NgaySinh.trim() === '') {
            $('#EditNgaySinhError').text('Ngày Sinh là trường bắt buộc');
            return;
        }
        var DiaChi = $('#EditDiaChi').val();
        if (DiaChi.trim() === '') {
            $('#EditDiaChiError').text('Địa chỉ là trường bắt buộc');
            return;
        }
        var DiemTB = $('#EditDiemTB').val();
        if (DiemTB.trim() === '') {
            $('#EditDiemTBError').text('Điểm trung bình là trường bắt buộc');
            return;
        }
        var Hs = {
            MaHS: $('#EditMaHS').val(),
            HoTen: HoTen,
            NgaySinh: NgaySinh,
            GioiTinh: $('#EditGioiTinh').is(':checked'),
            DiaChi: DiaChi,
            MaLop: $('#EditMaLop').val(),
            DiemTB: DiemTB,
        };
       
    $.ajax({
        type: 'POST',
    url: '/HocSinh/Edit',
    dataType: 'JSON',
    contentType: "application/json; charset=utf-8",
    data: JSON.stringify(Hs),
    success: function (response) {
        alert(response.status);
    LoadData();
                },
            });
        };

    function DeleteHocSinh(id) {
        $.ajax({
            type: 'POST',
            url: '/HocSinh/Delete',
            data: { id: id },
            success: function (response) {
                alert(response.status);
                LoadData();
            },
        });
};
function resetForm() {
    $('#quadratic-form input[type="text"]').not('#MaHS').val('');
    $('#quadratic-form input[type="date"]').val('');
    $('#quadratic-form select').val('');
    $('#quadratic-form input[type="checkbox"]').prop('checked', false);

    $('#HoTenError').text('');
    $('#NgaySinhError').text('');
    $('#DiaChiError').text('');
    $('#DiemTBError').text('');


    TangID();
};
function resetError() {
    $('#EditHoTenError').text('');
    $('#EditNgaySinhError').text('');
    $('#EditDiaChiError').text('');
    $('#EditDiemTBError').text('');
};
function XuLyNhapLieuADD() {
    // Xử lý sự kiện khi nhập liệu vào ô
    $('#HoTen').on('input', function () {
        $('#HoTenError').text('');
    });
    $('#NgaySinh').on('input', function () {
        $('#NgaySinhError').text('');
    });

    $('#DiaChi').on('input', function () {
        $('#DiaChiError').text('');
    });
    // Sự kiện rời khỏi ô báo lỗi
    $('#HoTen').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#HoTenError').text('Họ và tên là trường bắt buộc');
        } else {
            $('#HoTenError').text('');
        }
    });
    $('#NgaySinh').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#NgaySinhError').text('Ngày sinh là trường bắt buộc');
        } else {
            $('#NgaySinhError').text('');
        }
    });
    $('#DiaChi').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#DiaChiError').text('Địa chỉ là trường bắt buộc');
        } else {
            $('#DiaChiError').text('');
        }
    });
    $('#DiemTB').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#DiemTBError').text('Điểm trung bình là trường bắt buộc');
        } else {
            $('#DiemTBError').text('');
        }
    });
};

function XuLyNhapLieuEdit() {
    // Xử lý sự kiện khi nhập liệu vào ô
    $('#EditHoTen').on('input', function () {
        $('#EditHoTenError').text('');
    });
    $('#EditNgaySinh').on('input', function () {
        $('#EditNgaySinhError').text('');
    });

    $('#EditDiaChi').on('input', function () {
        $('#EditDiaChiError').text('');
    });
    // Sự kiện rời khỏi ô báo lỗi
    $('#EditHoTen').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#EditHoTenError').text('Họ và tên là trường bắt buộc');
        } else {
            $('#EditHoTenError').text('');
        }
    });
    $('#EditNgaySinh').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#EditNgaySinhError').text('Ngày sinh là trường bắt buộc');
        } else {
            $('#EditNgaySinhError').text('');
        }
    });
    $('#EditDiaChi').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#EditDiaChiError').text('Địa chỉ là trường bắt buộc');
        } else {
            $('#EditDiaChiError').text('');
        }
    });
    $('#EditDiemTB').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#EditDiemTBError').text('Điểm trung bình là trường bắt buộc');
        } else {
            $('#EditDiemTBError').text('');
        }
    });
};
