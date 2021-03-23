//window.alert('huh');
console.log('started review run');
let apiAddress2 = '', userRole;

const init = () => {
    const appOrigin2 = window.location.origin;
    apiAddress2 = appOrigin2 + '/api/ReportReview/';
}

const showLoading = () => {
    document.getElementById('reviewForm').style.display = 'none';
    document.getElementById('reviewTable').style.display = 'none';
    document.getElementById('reviewLoading').style.display = 'inline-block';
    document.getElementById('reviewLoading').innerHTML = 'Loading...';
}

const showReviewDetails = (data) => {
    document.getElementById('reviewLoading').style.display = 'none';
    document.getElementById('reviewTable').style.display = 'flex';
    document.getElementById('reviewDetails').innerHTML = data.details;
}

const showReviewSuccess = (details) => {
    document.getElementById('reviewLoading').innerHTML = 'Review successful!';
    setTimeout(() => {
        document.getElementById('reviewLoading').style.display = 'none';
        document.getElementById('reviewTable').style.display = 'flex';
        document.getElementById('reviewDetails').innerHTML = details;
    }, 3000);
}

const showReviewForm = () => {
    document.getElementById('reviewDetails').innerHTML = '';
    document.getElementById('reviewTable').style.display = 'none';
    document.getElementById('reviewLoading').style.display = 'none';
    document.getElementById('reviewForm').style.display = 'inline-block';
}

const showError = () => {
    document.getElementById('reviewLoading').style.display = 'inline-block';
    document.getElementById('reviewLoading').innerText =
        'Unable to fetch review, please refresh this page.'
        + '\nContact admin if problem persists.'
}

const showFailed = () => {
    document.getElementById('reviewLoading').innerHTML = 'Please try again...';
    setTimeout(() => {
        document.getElementById('reviewLoading').style.display = 'none';
        document.getElementById('reviewTable').style.display = 'none';
        document.getElementById('reviewForm').style.display = 'inline-block';
    }, 3000);
}

const requestGetReview = () => {
    showLoading();

    const pagePath = window.location.pathname.split('/');
    const itemId = pagePath[pagePath.length - 1];
    fetch(apiAddress2 + itemId)
        .then(response => response.json()
            .then(data => ({
                status: response.status,
                body: data
            })))
        .then(response => {
            if (response.status === 200) {
                showReviewDetails(response.body);
            } else if (response.status === 404) {
                showReviewForm();
            } else {
                console.log(response);
                showError();
            }
        })
        .catch(error => {
            console.log(error);
            showError();
    });
}

const startScript = () => {
    document.getElementById('reviewForm').style.display = 'none';
    document.getElementById('reviewTable').style.display = 'none';
    document.getElementById('reviewDetails').innerHTML = '';
    userRole = document.getElementById('roleOfCurrentUser').value;
    //console.log(userRole)
    if (userRole !== 'manager' 
        && userRole !== 'admin'
        && userRole !== 'superuser') {
            return;
    }

    init();
    requestGetReview();
}

const requestPostReview = () => {
    showLoading();
    
    if (userRole !== 'manager' 
    && userRole !== 'admin'
    && userRole !== 'superuser') {
        document.getElementById('reviewDetails').innerText =
        'Access denied!\nYou are not authorized to perform that action.';
        return;
    }

    let ownerId = document.getElementById('idOfCurrentUser').value;
    let xsrfToken = document.getElementById('__RequestVerificationToken').value;
    let taskId = document.getElementById('TaskReportId').value;
    let taskReviewDetails = document.getElementById('TaskReviewDetails').value;
    let formData = {
        OwnerId: ownerId,
        TaskReportId: parseInt(taskId),
        Details: taskReviewDetails
    }

    //console.log(formData);
    fetch(apiAddress2, {
        method: 'POST',
        body: JSON.stringify(formData),
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': xsrfToken,
        }
    })
    .then(response => response.json()
    .then(data => ({
        status: response.status,
        body: data
    })))
    .then(response => {
        if (response.status === 201) {
            showReviewSuccess(taskReviewDetails);
        } else {
            console.log(response);
            showFailed();
        }
    })
    .catch(error => {
        console.log(error);
        showFailed();
    });
}

startScript();
