$(document).ready(function () {
    $('#bookingForm').submit(function (event) {
        event.preventDefault(); // Ngăn chặn hành động mặc định của form

        // Lấy dữ liệu từ form
        var formData = {
            check_in: $('#checkInDatePicker').val(),
            check_out: $('#checkOutDatePicker').val()
        };

        // Gửi yêu cầu Ajax
        $.ajax({
            type: 'POST',
            url: '@Url.Action("search", "Room")', // Thay thế Tên_Controller bằng tên thực tế của controller
            data: formData,
            success: function (response) {
                // Xử lý kết quả nếu cần
                $('#searchRoom').html(response);
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                console.error('Lỗi khi gửi dữ liệu: ' + error);
            }
        });
    });
    // Bắt sự kiện click cho tất cả các thẻ <a> có class bookingLink
    $('.bookingLink').click(function (event) {
        event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>

        var roomId = $(this).data('room-id'); // Lấy id từ thuộc tính data-room-id của thẻ <a>

        var checkInDate = $('#checkInDatePicker').val();
        var checkOutDate = $('#checkOutDatePicker').val();

        if (!checkInDate || !checkOutDate) {
            // Ngày đến hoặc ngày đi chưa được nhập
            $('#checkInDatePicker').addClass('is-invalid');
            $('#checkOutDatePicker').addClass('is-invalid');
        } else {
            // Ngày đến và ngày đi đã được nhập
            $('#checkInDatePicker').removeClass('is-invalid');
            $('#checkOutDatePicker').removeClass('is-invalid');

            // Tạo URL sử dụng Url.Action và chuyển hướng đến đó
            var url = '@Url.Action("RoomService", "Room")' + '?id=' + roomId;
            window.location.href = url;
        }
    });

});
$(document).ready(function () {
    $('.room-item').click(function () {
        var roomId = $(this).data('room-id');
        $.ajax({
            url: '@Url.Action("RoomDetails", "Room")',
            type: 'GET',
            data: { id: roomId },
            success: function (result) {
                $('#room-detail-container').fadeOut(0).delay(200).html(result).fadeIn(200);
            }
        });
    });

});