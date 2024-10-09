
function goToEditProduct() {
    
    var selectedProductId = sessionStorage.getItem('selectedProductId');
    if (selectedProductId) {
        
        window.location.href = "/Products/edit/" + selectedProductId;
    } else {
        alert('Vui lòng chọn một sản phẩm trước khi chỉnh sửa.');
    }
}
function goToDeleteProduct() {
    
    var selectedProductId = sessionStorage.getItem('selectedProductId');
    if (selectedProductId) {
        
        window.location.href = "/Products/delete/" + selectedProductId;
    } else {
        alert('Vui lòng chọn một sản phẩm trước khi chỉnh sửa.');
    }
}

function handleClickIdDataCell(event) {
    
    var targetCell = event.currentTarget; 
    
    if (targetCell.classList.contains('selectedProductId')) {
        
        targetCell.classList.remove('selectedProductId');
        sessionStorage.removeItem('selectedProductId');
    } else {
            
        var idDataCells = document.querySelectorAll('.ProductId');
        idDataCells.forEach(function (cell) {
            cell.classList.remove('selectedProductId');
        });
        targetCell.classList.add('selectedProductId');

        
        var id = targetCell.textContent.trim();
        sessionStorage.setItem('selectedProductId', id);
    }
}

function handleClickUserId(event) {

    var targetCell = event.currentTarget;

    if (targetCell.classList.contains('selectedUserId')) {

        targetCell.classList.remove('selectedUserId');
        sessionStorage.removeItem('selectedUserId');
    } else {

        var idDataCells = document.querySelectorAll('.UserId');
        idDataCells.forEach(function (cell) {
            cell.classList.remove('selectedUserId');
        });
        targetCell.classList.add('selectedUserId');


        var id = targetCell.textContent.trim();
        sessionStorage.setItem('selectedUserId', id);
    }
}


