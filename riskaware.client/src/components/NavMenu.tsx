import { Component, ContextType } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import AuthContext from '../auth/AuthContext';

export class NavMenu extends Component<object, { collapsed: boolean }> {
  static displayName = NavMenu.name;
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    if (!this.context) {
      return null;
    }
    const { isLoggedIn, logout } = this.context;

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">RiskAware</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          {isLoggedIn && (
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Všechny projekty</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/myProjects">Vlastní projekty</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/login" onClick={logout}>Odhlásit se</NavLink>
                </NavItem>
              </ul>
            </Collapse>
          )}
        </Navbar>
      </header>
    );
  }
}
