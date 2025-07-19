function showToast(title, message, isSuccess) {
    const toast = new bootstrap.Toast($('#nameToast')[0]);
    $('#toastTitle').text(title);
    $('#toastMessage').text(message);
    $('#nameToast').removeClass('bg-success bg-danger text-white').addClass(isSuccess ? 'bg-success text-white' : 'bg-danger text-white');
    toast.show();
}