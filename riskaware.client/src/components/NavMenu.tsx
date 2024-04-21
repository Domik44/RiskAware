import { Component, ContextType } from 'react';
import { Collapse, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import AuthContext from '../auth/AuthContext';

interface NavMenuState {
  collapsed: boolean;
  userDropOpen: boolean;
}

export class NavMenu extends Component<object, NavMenuState> {
  static displayName = NavMenu.name;
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      userDropOpen: false,
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  toggleUserDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      userDropOpen: !prevState.userDropOpen,
    }));
  };

  render() {
    if (!this.context) {
      return null;
    }

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">RiskAware</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          {this.context.isLoggedIn && (
            <Collapse className="d-sm-inline-flex flex-sm-row" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Všechny projekty</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/myProjects">Vlastní projekty</NavLink>
                </NavItem>
              </ul>

              <ul className="navbar-nav ms-auto">
                <Dropdown isOpen={this.state.userDropOpen} toggle={this.toggleUserDropdown}>
                  <DropdownToggle nav caret>
                    Přihlášen jako: {this.context.email}
                  </DropdownToggle>
                  <DropdownMenu>
                    <DropdownItem tag={Link} onClick={this.context.logout}>Odhlásit se</DropdownItem>
                  </DropdownMenu>
                </Dropdown>
              </ul>
            </Collapse>
          )}
        </Navbar>
      </header>
    );
  }
}
