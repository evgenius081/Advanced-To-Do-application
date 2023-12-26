import { Component } from '@angular/core';
import {
  faStar,
  faClock,
  faBoxArchive,
  faPlus,
  faRightFromBracket,
} from '@fortawesome/free-solid-svg-icons';
import { TodoListWithStatistics } from '../../shared/classes/todo-list-with-statistics';
import { ListService } from '../../shared/services/list.service';
import { UserService } from '../../shared/services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss'],
})
export class SideMenuComponent {
  faStar = faStar;
  faClock = faClock;
  faBoxArchive = faBoxArchive;
  faPlus = faPlus;
  faRightFromBracket = faRightFromBracket;
  icon = false;
  lists: TodoListWithStatistics[] = [];

  constructor(
    private listService: ListService,
    private userService: UserService,
    private router: Router
  ) {}

  click() {
    this.icon = !this.icon;
  }

  ngOnInit() {
    this.getLists();
  }

  getLists() {
    this.listService.getLists().subscribe((lists) => (this.lists = lists));
  }

  findNotArchived(list: TodoListWithStatistics): boolean {
    return !list.isArchived;
  }

  findArchived(list: TodoListWithStatistics): boolean {
    return list.isArchived;
  }

  handleLogout() {
    this.userService.logout();
    this.router.navigate(['/login']);
  }
}
