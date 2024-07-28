document.addEventListener("DOMContentLoaded", function () {
    var inputs = [
        { id: "inputName", label: "Tên khách hàng" },
        { id: "inputEmail", label: "Email của bạn" },
        { id: "inputPassword", label: "Mật khẩu của bạn" },
        { id: "inputPhone", label: "Số điện thoại của bạn" }
    ];

    inputs.forEach(function (input) {
        var element = document.getElementById(input.id);
        var label = document.querySelector("label[for='" + input.id + "']");

        function toggleLabel() {
            if (element.value === "") {
                label.style.display = "block";
            } else {
                label.style.display = "none";
            }
        }

        element.addEventListener("focus", toggleLabel);
        element.addEventListener("input", toggleLabel);
        element.addEventListener("blur", toggleLabel);

        // Initialize the label display based on initial value
        toggleLabel();
    });
});