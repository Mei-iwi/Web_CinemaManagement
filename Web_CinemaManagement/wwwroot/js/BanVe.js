
    var cart = [];

    // --- 1. Hàm thêm vé ---
    function themVe() {
        var maGhe = $('#ddlGhe').val();
    var maLV = $('#ddlLoaiVe').val();
    var tenLV = $('#ddlLoaiVe option:selected').text();

    // Validate
    if (!maGhe) {alert("Vui lòng chọn ghế trước!"); return; }
        if (cart.find(x => x.maGhe == maGhe)) {alert("Ghế này đã có trong giỏ hàng!"); return; }

    // Thêm vào mảng
    cart.push({maGhe: maGhe, maLV: maLV, tenLV: tenLV });

    // Cập nhật giao diện
    render();
    updateData();

    // Ẩn ghế đã chọn khỏi dropdown
    $("#ddlGhe option[value='" + maGhe + "']").hide();
    $('#ddlGhe').val(""); // Reset về default
    }

    // --- 2. Hàm xóa vé ---
    function xoaVe(maGhe) {
        if (!confirm("Bạn có chắc muốn xóa ghế " + maGhe + "?")) return;

        cart = cart.filter(x => x.maGhe !== maGhe);

    // Hiện lại ghế trong dropdown
    $("#ddlGhe option[value='" + maGhe + "']").show();

    render();
    updateData();
    }

    // --- 3. Hàm vẽ lại bảng giỏ hàng ---
    function render() {
        var tbody = $('#cart-display');
    tbody.empty();

    if (cart.length === 0) {
        $('#empty-cart-msg').show();
        } else {
        $('#empty-cart-msg').hide();
            cart.forEach(x => {
                var row = `<tr>
        <td class="font-weight-bold text-success">${x.maGhe}</td>
        <td>${x.tenLV}</td>
        <td class="text-right">
            <button type="button" class="btn btn-sm btn-danger py-0" onclick="xoaVe('${x.maGhe}')">
                <i class="fa fa-times"></i>
            </button>
        </td>
    </tr>`;
    tbody.append(row);
            });
        }
    }

    // --- 4. CẬP NHẬT DỮ LIỆU GỬI ĐI (FIX LỖI) ---
    function updateData() {
        // Xử lý chuỗi ghế
        if (cart.length > 0) {
            var strGhe = cart.map(x => x.maGhe + "_" + x.maLV).join(";");
    $('#hiddenStrGhe').val(strGhe);
        } else {
        $('#hiddenStrGhe').val("");
        }

    // Xử lý chuỗi dịch vụ (Kiểm tra kỹ số lượng)
    var s = [];
    $('.service-qty').each(function () {
            var val = $(this).val();
    var q = parseInt(val); // Chuyển sang số nguyên

            // Chỉ lấy nếu là số và > 0 (tránh NaN, null, undefined)
            if (!isNaN(q) && q > 0) {
                var id = $(this).data('id');
    s.push(id + "_" + q);
            }
        });

    $('#hiddenStrDichVu').val(s.join(";"));
    }