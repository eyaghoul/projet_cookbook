using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookBook.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecetteIngredients");

            migrationBuilder.DropTable(
                name: "Recettes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesCuisine",
                table: "TypesCuisine");

            migrationBuilder.DropColumn(
                name: "CalParUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Unite",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "TypesCuisine",
                newName: "TypeCuisines");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Ingredients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "TypeCuisines",
                newName: "Name");

            migrationBuilder.AddColumn<double>(
                name: "CaloriesPerUnit",
                table: "Ingredients",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ingredients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Ingredients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeCuisines",
                table: "TypeCuisines",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NumberOfPersons = table.Column<int>(type: "INTEGER", nullable: false),
                    CookingMethod = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    PrepTimeMinutes = table.Column<int>(type: "INTEGER", nullable: true),
                    CookTimeMinutes = table.Column<int>(type: "INTEGER", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeCuisineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_TypeCuisines_TypeCuisineId",
                        column: x => x.TypeCuisineId,
                        principalTable: "TypeCuisines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Abbreviation = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<double>(type: "REAL", precision: 10, scale: 3, nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => new { x.RecipeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Petit-déjeuner" },
                    { 2, "Déjeuner" },
                    { 3, "Dîner" },
                    { 4, "Dessert" }
                });

            migrationBuilder.InsertData(
                table: "TypeCuisines",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tunisienne" },
                    { 2, "Française" },
                    { 3, "Italienne" },
                    { 4, "Méditerranéenne" },
                    { 5, "Américaine" },
                    { 6, "Asiatique" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Abbreviation", "Name" },
                values: new object[,]
                {
                    { 1, "g", "Gramme" },
                    { 2, "kg", "Kilogramme" },
                    { 3, "ml", "Millilitre" },
                    { 4, "L", "Litre" },
                    { 5, "c.s", "Cuillère à soupe" },
                    { 6, "c.c", "Cuillère à café" },
                    { 7, "pc", "Pièce" },
                    { 8, "t", "Tasse" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UnitId",
                table: "Ingredients",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_UnitId",
                table: "RecipeIngredients",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_TypeCuisineId",
                table: "Recipes",
                column: "TypeCuisineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_UnitId",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeCuisines",
                table: "TypeCuisines");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TypeCuisines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TypeCuisines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TypeCuisines",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TypeCuisines",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TypeCuisines",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TypeCuisines",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "CaloriesPerUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "TypeCuisines",
                newName: "TypesCuisine");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Ingredients",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TypesCuisine",
                newName: "Nom");

            migrationBuilder.AddColumn<decimal>(
                name: "CalParUnit",
                table: "Ingredients",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Unite",
                table: "Ingredients",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesCuisine",
                table: "TypesCuisine",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Recettes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategorieId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeCuisineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NbPersonnes = table.Column<int>(type: "INTEGER", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recettes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recettes_Categories_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recettes_TypesCuisine_TypeCuisineId",
                        column: x => x.TypeCuisineId,
                        principalTable: "TypesCuisine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecetteIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecetteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantite = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetteIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecetteIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecetteIngredients_Recettes_RecetteId",
                        column: x => x.RecetteId,
                        principalTable: "Recettes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecetteIngredients_IngredientId",
                table: "RecetteIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetteIngredients_RecetteId",
                table: "RecetteIngredients",
                column: "RecetteId");

            migrationBuilder.CreateIndex(
                name: "IX_Recettes_CategorieId",
                table: "Recettes",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_Recettes_TypeCuisineId",
                table: "Recettes",
                column: "TypeCuisineId");
        }
    }
}
