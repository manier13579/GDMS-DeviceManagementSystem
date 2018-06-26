function Logout(){
  sessionStorage.removeItem('access_token');
  sessionStorage.removeItem('userid');
  sessionStorage.removeItem('username');
  sessionStorage.removeItem('role');
  sessionStorage.removeItem('viewNow');
  location.href = 'account';
}