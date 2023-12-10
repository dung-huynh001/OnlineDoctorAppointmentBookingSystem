// This method will cancel the appointment with id equals to data-appointmentid of CancelAppointment Button
$(document).on('click', '#btnCancel', function () {
    var btnCancel = $(this);
    var appointmentID = btnCancel.data('appointmentid');

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success mr-2',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        position: 'top',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, cancel it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Doctor/Appointments/CancelAppointment',
                method: 'POST',
                data: { id: appointmentID },
                dataType: 'JSON',
                success: function (res) {
                    if (res.success) {
                        Swal.fire({
                            title: 'Cancelled!',
                            text: 'Appointment has been cancelled.',
                            icon: 'success',
                            position: 'top',
                            showConfirmButton: false,
                            timer: 3000
                        })
                        setTimeout(function () {
                            window.location.href = '/Doctor/CancelledAppt/Index';
                        }, 3000)
                    } else {
                        console.log(res.message)
                    }

                },
                error: function (err) {
                    console.log(err.responseText);
                }
            })
        }
    })
})

// This method will cancel the appointment with id equals to data-appointmentid of CancelAppointment Button
$(document).on('click', '#btnConfirm', function () {
    var btnConfirm = $(this);
    var appointmentID = btnConfirm.data('appointmentid');

    $.ajax({
        url: '/Doctor/Appointments/ConfirmAppointment',
        method: 'POST',
        data: { id: appointmentID },
        dataType: 'JSON',
        success: function (res) {
            if (res.success) {
                Swal.fire({
                    title: 'Success!',
                    text: 'Appointment has been confirmed.',
                    icon: 'success',
                    position: 'top',
                    showConfirmButton: false,
                    timer: 3000
                })
                setTimeout(function () {
                    window.location.href = '/Doctor/ConfirmedAppt/Index';
                }, 3000)
            } else {
                console.log(res.message)
            }
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
})